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
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FacebookService));
        
        private readonly ICache _cache;
        private readonly string _appId;
        private readonly string _appSecret;
        private readonly string _token;
        
        private const string FacebookFeedFields = "&fields = caption, id, created_time, message,from, name, type, is_published, shares";
        private string FacebookTokenUrl => $"https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={_appId}&client_secret={_appSecret}";
        private string AuthenticationHeader => $"Bearer {_token}";
        private bool _useCache = true;
        private int _cacheDurationInMinutes = 10;

        public FacebookService(ICache cache, string appId, string appSecret)
        {
            _cache = cache;
            _appId = appId;
            _appSecret = appSecret;
            
            _token = GetAppToken();
        }

        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            _useCache = useCache;
            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public FacbookAuthorInformation GetInformation(string userName)
        {
            if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(userName))
            {
                return null;
            }

            var facebookInfoCacheKey = $"facebook_about_{userName}";
            if (_cache.Exists(facebookInfoCacheKey) && _useCache)
            {
                return _cache.Get<FacbookAuthorInformation>(facebookInfoCacheKey);
            }

            var facebookInfoUrl = $"https://graph.facebook.com/v2.5/{userName}";
            try
            {
                var text = HttpUtils.Get(facebookInfoUrl, AuthenticationHeader);
                var data = JsonConvert.DeserializeObject<FacbookAuthorInformation>(text);

                if (_useCache && data != null)
                {
                    _cache.Add(facebookInfoCacheKey, data, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest)
        {
            if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(facebookFeedRequest.UserName))
            {
                return null;
            }

            var facebookFeedCacheKey = $"facebook_feed_{facebookFeedRequest.UserName}_{facebookFeedRequest.MaxCount}";
            if (_cache.Exists(facebookFeedCacheKey) && _useCache)
            {
                return _cache.Get<FacebookFeedResponse>(facebookFeedCacheKey);
            }

            var facebookFeedUrl = $"https://graph.facebook.com/v2.5/{facebookFeedRequest.UserName}/posts?limit={facebookFeedRequest.MaxCount}{FacebookFeedFields}";
            try
            {
                var posts = HttpUtils.Get(facebookFeedUrl, AuthenticationHeader);
                var data = JsonConvert.DeserializeObject<FacebookFeedResponse>(posts);

                if (_useCache && data != null)
                {
                    _cache.Add(facebookFeedCacheKey, data, new TimeSpan(0, _cacheDurationInMinutes, 0));
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
            if (string.IsNullOrEmpty(_appId) || string.IsNullOrEmpty(_appSecret))
            {
                return string.Empty;
            }

            var facebookTokenCacheKey = $"facebook_token_{_appId}";
            if (_cache.Exists(facebookTokenCacheKey) && _useCache)
            {
                return _cache.Get<string>(facebookTokenCacheKey);
            }

            var token = GetAppTokenFromFacebook();
            if (_useCache && !string.IsNullOrEmpty(token))
            {
                _cache.Add(token, facebookTokenCacheKey, new TimeSpan(0, _cacheDurationInMinutes, 0));
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

            if (!string.IsNullOrEmpty(token))
            {
                var tokenObject = JsonConvert.DeserializeObject<dynamic>(token);
                token = tokenObject["access_token"].ToString();
            }

            return token;
        }
    }
}