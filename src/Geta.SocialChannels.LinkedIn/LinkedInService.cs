﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.WebPages;
using EPiServer.Data.Dynamic;
using EPiServer.Logging;
using EPiServer.Web;

namespace Geta.SocialChannels.LinkedIn
{
    public class LinkedInService : ILinkedInService
    {
        /// <summary>
        ///     The authorization url to get authorization code.
        /// </summary>
        private const string UrlAuthorization =
            "https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id={0}&state={1}&redirect_uri={2}";

        /// <summary>
        ///     The url to exchange access token from authorization code.
        /// </summary>
        private const string UrlExchangeAccessToken =
            "https://www.linkedin.com/uas/oauth2/accessToken?grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}";

        /// <summary>
        ///     The url to get company feeds.
        /// </summary>
        private const string UrlCompanyPageFeeds =
            "https://api.linkedin.com/v1/companies/{0}/updates?count={1}&start={2}&format=json";

        private static readonly ILogger Logger = LogManager.GetLogger();

        private readonly ICache _cache;

        private readonly int _cacheDuration;

        private readonly int _numberFeedItems;

        private readonly StartPage _startPage;

        public LinkedInService(ICache cache)
        {
            this._cache = cache;

            //_startPage = ContentReference.StartPage.Get<StartPage>();
            //_numberFeedItems = _startPage.SocialFeedSettings.NumberSocialFeedItems > 0
            //    ? _startPage.SocialFeedSettings.NumberSocialFeedItems
            //    : Constants.DefaultNumberSocialFeedItems;
            //_cacheDuration = _startPage.SocialFeedSettings.SocialFeedCacheDuration > 0
            //    ? _startPage.SocialFeedSettings.SocialFeedCacheDuration
            //    : Constants.DefaultSocialFeedCacheDurationForMinutes;
        }

        /// <summary>
        ///     Get access token from DDS.
        /// </summary>
        public LinkedInAccessTokenViewModel GetAccessTokenData(LinkedInFeedBlock linkedInFeedBlock)
        {
            try
            {
                var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(AccessTokenData));
                var accessTokenData = (from at in store.Items<AccessTokenData>()
                                       where at.EntityId == linkedInFeedBlock.Guid
                                       select at).FirstOrDefault();

                // If null or expire
                if (accessTokenData == null)
                {
                    // Create new access token.
                    accessTokenData = new AccessTokenData(linkedInFeedBlock, string.Empty, string.Empty, DateTime.Now);
                    accessTokenData.Save<AccessTokenData>();
                }
                else
                {
                    // Update clientId and client secret.
                    var isUpdate = false;
                    if (accessTokenData.ClientId != linkedInFeedBlock.ClientId)
                    {
                        accessTokenData.ClientId = linkedInFeedBlock.ClientId;
                        isUpdate = true;
                    }

                    if (accessTokenData.ClientSecret != linkedInFeedBlock.ClientSecret)
                    {
                        accessTokenData.ClientSecret = linkedInFeedBlock.ClientSecret;
                        isUpdate = true;
                    }

                    if (accessTokenData.CompanyId != linkedInFeedBlock.CompanyId)
                    {
                        accessTokenData.CompanyId = linkedInFeedBlock.CompanyId;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        accessTokenData.ResetAccessToken();
                        accessTokenData.Update<AccessTokenData>();
                    }
                }

                var accessTokenViewModel = new LinkedInAccessTokenViewModel(accessTokenData);

                return accessTokenViewModel;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        ///     Get authorization url.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="guid">The unique string to prevent csrf.</param>
        /// <returns></returns>
        public Uri GetAuthorizationUrl(string clientId, Guid guid)
        {
            try
            {
                var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(AccessTokenData));
                var accessTokenData = (from at in store.Items<AccessTokenData>()
                                       where at.EntityId == guid
                                       select at).FirstOrDefault();

                // Update expire request time.
                if (accessTokenData != null)
                {
                    accessTokenData.ExpireRequestTime = DateTime.Now.AddSeconds(10);
                    accessTokenData.Update<AccessTokenData>();
                }

                var url = string.Format(UrlAuthorization, clientId, guid, GetRedirectUrl());

                return new Uri(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        ///     Exchange authorization code for access token.
        /// </summary>
        public async Task<LinkedInAccessTokenViewModel> ExchangeAccessTokenAsync(string authorizationCode,
            string clientId, string clientSecet)
        {
            try
            {
                LinkedInAccessTokenViewModel accessToken = null;
                var url = string.Format(UrlExchangeAccessToken, authorizationCode, GetRedirectUrl(), clientId,
                    clientSecet);

                using (var httpClient = new HttpClient())
                {
                    var httpResponse = await httpClient.PostAsync(url, null);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        accessToken = await httpResponse.Content.ReadAsAsync<LinkedInAccessTokenViewModel>();
                    }
                    else
                    {
                        var error = await httpResponse.Content.ReadAsAsync<LinkedinExceptionViewModel>();

                        Logger.Error(error.Description);
                    }
                }

                return accessToken;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);

                return null;
            }
        }

        /// <summary>
        ///     Get linkedin company feeds.
        /// </summary>
        /// <returns></returns>
        public Task<LinkedInViewModel> GetFeedsAsync(string accessToken, string companyId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var key = $"linkedin_feed_{companyId}_{_numberFeedItems}_{_cacheDuration}";

                    if (_cache.Exists(key)) //&& _startPage.SocialFeedSettings.EnableSocialFeedCache)
                    {
                        return _cache.Get<LinkedInViewModel>(key);
                    }

                    LinkedInViewModel linkedInViewModel;
                    var offset = 0;

                    var tempValues = new List<LinkedInValuesViewModel>();
                    while (true)
                    {
                        linkedInViewModel = await GetFeedsAsync(accessToken, companyId, offset);

                        if (linkedInViewModel != null)
                        {
                            linkedInViewModel.Values = linkedInViewModel.Values.Where(p => p.IsShowOnView).ToList();
                            tempValues.AddRange(linkedInViewModel.Values);

                            if (linkedInViewModel.Total <= _numberFeedItems + offset)
                            {
                                break;
                            }

                            if (tempValues.Count >= _numberFeedItems)
                            {
                                break;
                            }

                            offset += _numberFeedItems;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (linkedInViewModel != null)
                    {
                        linkedInViewModel.Values = tempValues;

                        if (linkedInViewModel.Values.Count > _numberFeedItems)
                        {
                            linkedInViewModel.Values = linkedInViewModel.Values.Take(_numberFeedItems).ToList();
                        }
                    }

                    if (linkedInViewModel != null)//_startPage.SocialFeedSettings.EnableSocialFeedCache && 
                    {
                        _cache.Add(key, linkedInViewModel, new TimeSpan(0, _cacheDuration, 0));
                    }

                    return linkedInViewModel;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);

                    return null;
                }
            });
        }

        private async Task<LinkedInViewModel> GetFeedsAsync(string accessToken, string companyId, int offset)
        {
            LinkedInViewModel linkedInViewModel = null;
            var url = string.Format(UrlCompanyPageFeeds, companyId, _numberFeedItems, offset);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var httpResponse = await httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    linkedInViewModel = await httpResponse.Content.ReadAsAsync<LinkedInViewModel>();
                }
                else
                {
                    var error = await httpResponse.Content.ReadAsAsync<LinkedinExceptionViewModel>();

                    Logger.Error(error.Description);
                }
            }

            return linkedInViewModel;
        }

        /// <summary>
        ///     Get redirect url.
        /// </summary>
        /// <returns></returns>
        private static string GetRedirectUrl()
        {
            var url = SiteDefinition.Current.SiteUrl.ToString().EndsWith("/")
                ? SiteDefinition.Current.SiteUrl.ToString()
                : SiteDefinition.Current.SiteUrl + "/";
            return $"{url}sociallistener/linkedin";
        }
    }
}