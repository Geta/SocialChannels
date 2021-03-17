using System;
using System.Linq;
using Geta.SocialChannels.Facebook.DTO;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Facebook
{
    public class FacebookService : IFacebookService
    {
        private const string BaseUrl = "https://graph.facebook.com/v10.0/";
        private readonly string _token;
        private readonly ICache _cache;
        
        private bool _useCache;
        private int _cacheDurationInMinutes = 10;

        public FacebookService(string token, ICache cache = null)
        {
            _token = token;
            _cache = cache;
            _useCache = cache != null;
        }
        
        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            _useCache = useCache;
            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        /// <summary>
        /// Gets account information for specified username.
        /// </summary>
        public FacebookAccountInformation GetInformation(string userName)
        {
            if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(userName))
            {
                return null;
            }
            
            var facebookInfoCacheKey = $"facebook_about_{userName}";
            if (_useCache && _cache.Exists(facebookInfoCacheKey))
            {
                return _cache.Get<FacebookAccountInformation>(facebookInfoCacheKey);
            }

            try
            {
                var fields = "about,description,id,members,location,phone,website,username";
                var url = $"{BaseUrl}{userName}?fields={fields}&access_token={_token}";
                var jsonResult = HttpUtils.Get(url);
                var accountInformationResponse = JsonConvert.DeserializeObject<AccountInformationDto>(jsonResult);

                return new FacebookAccountInformation
                {
                    Id = accountInformationResponse.Id,
                    Username = accountInformationResponse.Username,
                    Description = accountInformationResponse.Description,
                    Phone = accountInformationResponse.Phone,
                    Website = accountInformationResponse.Website,
                    Location = accountInformationResponse.Location != null ? new Location
                    {
                        City = accountInformationResponse.Location.City,
                        Country = accountInformationResponse.Location.Country,
                        Latitude = accountInformationResponse.Location.Latitude,
                        Longitude = accountInformationResponse.Location.Longitude,
                        Street = accountInformationResponse.Location.Street,
                        Zip = accountInformationResponse.Location.Zip
                    } : null
                };
            }
            catch (Exception e)
            {
                throw new FacebookServiceException(e.Message, e);
            }
        }

        /// <summary>
        /// Gets Facebook feed by username.
        /// </summary>
        public FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest)
        {
            if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(facebookFeedRequest.UserName))
            {
                return null;
            }

            var facebookFeedCacheKey = $"facebook_feed_{facebookFeedRequest.UserName}_{facebookFeedRequest.MaxCount}";
            if (_useCache && _cache.Exists(facebookFeedCacheKey))
            {
                return _cache.Get<FacebookFeedResponse>(facebookFeedCacheKey);
            }

            try
            {
                var fields = "message,created_time,attachments{url,description,media_type}";
                var url = $"{BaseUrl}{facebookFeedRequest.UserName}/feed?fields={fields}&access_token={_token}";
                var jsonResult = HttpUtils.Get(url);
                var feedDto = JsonConvert.DeserializeObject<FeedDto>(jsonResult);

                return new FacebookFeedResponse
                {
                    Data = feedDto?.Data.Select(s => new FacebookPostItem
                    {
                        Id = s.Id,
                        Message = s.Message,
                        CreatedTime = s.CreatedTime,
                        Attachments = s.Data?.Attachments.Select(a => new FacebookAttachment
                        {
                            Description = a.Description,
                            MediaType = a.MediaType,
                            Url = a.Url
                        }).ToList()
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                throw new FacebookServiceException(e.Message, e);
            }
        }
    }
}