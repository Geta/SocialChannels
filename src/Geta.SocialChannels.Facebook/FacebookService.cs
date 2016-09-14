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

        private const string FacebookFeedFields = "&fields = caption, id, created_time, message,from, name, type, is_published, shares";

        private string FacebookTokenUrl => $"https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={_appId}&client_secret={_appSecret}";
        private string FacebookTokenCacheKey => $"facebook_token_{_appId}";

        private string AuthenticationHeader => $"OAuth {_token}";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(FacebookService));
        private readonly ICache _cache;

        private readonly string _appId;
        private readonly string _appSecret;
        private readonly string _facebookId;
        private readonly string _token;

        private bool _useCache = true;
        private int _cacheDurationInMinutes = 10;

        public FacebookService(ICache cache, string appId, string appSecret, string facebookId)
        {
            this._cache = cache;
            this._appId = appId;
            this._appSecret = appSecret;
            this._facebookId = facebookId;

            _token = GetAppToken();
        }

        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            this._useCache = useCache;
            this._cacheDurationInMinutes = cacheDurationInMinutes;
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
                    _cache.Add(FacebookInfoCacheKey, data, new TimeSpan(0, _cacheDurationInMinutes, 0));
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
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            string facebookFeedCacheKey = $"facebook_feed_{_facebookId}_{facebookFeedRequest.MaxCount}";

            if (_cache.Exists(facebookFeedCacheKey) && _useCache)
            {
                return _cache.Get<FacebookFeedResponse>(facebookFeedCacheKey);
            }

            string facebookFeedUrl = $"https://graph.facebook.com/v2.5/{_facebookId}/posts?limit={facebookFeedRequest.MaxCount}{FacebookFeedFields}";

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
                _cache.Add(token, FacebookTokenCacheKey, new TimeSpan(0, _cacheDurationInMinutes, 0));
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