using System;
using System.Linq;
using System.Reflection;
using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Facebook
{
    [ServiceConfiguration(typeof(IFacebookService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class FacebookService : IFacebookService
    {

        private string FacebookInfoCacheKey => $"facebook_about_{_facebookId}";
        private string FacebookInfoUrl => $"https://graph.facebook.com/v2.5/{_facebookId}";

        private string FacebookFeedCacheKey => $"facebook_feed_{_facebookId}_{_numberFeedItems}";
        private const string FacebookFeedFields = "&fields = caption, id, created_time, message,from, name, type, is_published, shares";
        private string FacebookFeedUrl => $"https://graph.facebook.com/v2.5/{_facebookId}/posts?limit={_numberFeedItems}{FacebookFeedFields}";

        private string FacebookTokenUrl => $"https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={_appId}&client_secret={_appSecret}";
        private string FacebookTokenCacheKey => $"facebook_token_{_appId}";

        private string AuthenticationHeader => $"OAuth {_token}";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(FacebookService));
        private readonly ICache _cache;

        private string _appId;
        private string _appSecret;
        private string _facebookId;
        private string _token;
        private readonly bool _useCache;
        private readonly int _cacheDuration;
        private readonly int _numberFeedItems;

        public FacebookService(ICache cache)
        {
            this._cache = cache;

            //var startPage = ContentReference.StartPage.Get<StartPage>();
            //_numberFeedItems = startPage.SocialFeedSettings.NumberSocialFeedItems > 0 ? startPage.SocialFeedSettings.NumberSocialFeedItems : Constants.DefaultNumberSocialFeedItems;
            //_cacheDuration = startPage.SocialFeedSettings.SocialFeedCacheDuration > 0 ? startPage.SocialFeedSettings.SocialFeedCacheDuration : Constants.DefaultSocialFeedCacheDurationForMinutes;
            //_useCache = startPage.SocialFeedSettings.EnableSocialFeedCache;
        }

        public void Config(string appId, string appSecret, string facebookId)
        {
            _appId = appId;
            _appSecret = appSecret;
            _facebookId = facebookId;
            _token = GetAppToken();
        }

        public FacbookAuthorInformation GetInformation()
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            if (_cache.Exists(FacebookInfoCacheKey) && _useCache)
            {
                return _cache.Get<FacbookAuthorInformation>(FacebookInfoCacheKey);
            }

            try
            {
                var text = HttpUtils.Get(FacebookInfoUrl, AuthenticationHeader);
                var data = JsonConvert.DeserializeObject<FacbookAuthorInformation>(text);

                if (_useCache && data != null)
                {
                    _cache.Add(FacebookInfoCacheKey, data, new TimeSpan(0, _cacheDuration, 0));
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);

                return null;
            }
        }

        public FacebookFeed GetFacebookFeed()
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            if (_cache.Exists(FacebookFeedCacheKey) && _useCache)
            {
                return _cache.Get<FacebookFeed>(FacebookFeedCacheKey);
            }

            try
            {
                var posts = HttpUtils.Get(FacebookFeedUrl, AuthenticationHeader);
                var data = JsonConvert.DeserializeObject<FacebookFeed>(posts);

                if (_useCache && data != null)
                {
                    _cache.Add(FacebookFeedCacheKey, data, new TimeSpan(0, _cacheDuration, 0));
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        private string GetAppToken()
        {
            if (string.IsNullOrEmpty(_appId)
                    || string.IsNullOrEmpty(_appSecret)
                    || string.IsNullOrEmpty(_facebookId))
            {
                return string.Empty;
            }

            if (_cache.Exists(FacebookTokenCacheKey) && _useCache)
            {
                return _cache.Get<string>(FacebookTokenCacheKey);
            }

            var token = GetAppTokenFromFacebook();

            if (_useCache && !string.IsNullOrEmpty(token))
            {
                _cache.Add(token, FacebookTokenCacheKey, new TimeSpan(0, _cacheDuration, 0));
            }

            return token;
        }

        private string GetAppTokenFromFacebook()
        {
            string token;
            try
            {
                token = HttpUtils.Get(FacebookTokenUrl);
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(token) && token.Contains('='))
            {
                token = token.Split('=')[1];
            }

            return token;
        }
    }
}