using System;
using System.Reflection;
using System.Threading.Tasks;
using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Instagram
{
    [ServiceConfiguration(typeof(IInstagramService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class InstagramService: IInstagramService
    {
        private const string SelfApiEndpoint = "https://api.instagram.com/v1/users/self/media/recent/?access_token={0}&count={1}";
        private const string UsersApiEndpoint = "https://api.instagram.com/v1/users/{0}/media/recent/?access_token={1}&count={2}";
        private const string TagsApiEndpoint = "https://api.instagram.com/v1/tags/{0}/media/recent/?access_token={1}&count={2}";
        
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InstagramService));
        private readonly ICache _cache;
        private readonly string _token;
        
        private bool _useCache = true;
        private int _cacheDurationInMinutes = 10;

        public InstagramService(ICache cache, string token)
        {
            _cache = cache;
            _token = token;
        }
        
        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            _useCache = useCache;
            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public InstagramResponse GetPostsBySelf(int maxCount)
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            var instagramSelfCacheKey = $"instagram_self_{_token}";
            if (_useCache && _cache.Exists(instagramSelfCacheKey))
            {
                return _cache.Get<InstagramResponse>(instagramSelfCacheKey);
            }

            try
            {
                var textResponse = HttpUtils.Get(string.Format(SelfApiEndpoint, _token, maxCount));
                var response = JsonConvert.DeserializeObject<InstagramResponse>(textResponse);

                if (_useCache && response != null)
                {
                    _cache.Add(instagramSelfCacheKey, response, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public InstagramResponse GetPostsByUser(GetPostsRequest request)
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            var instagramUserCacheKey = $"instagram_user_{request.Query}_{request.MaxCount}";
            if (_useCache && _cache.Exists(instagramUserCacheKey))
            {
                return _cache.Get<InstagramResponse>(instagramUserCacheKey);
            }

            try
            {
                var textResponse = HttpUtils.Get(string.Format(UsersApiEndpoint, request.Query, _token, request.MaxCount));
                var response = JsonConvert.DeserializeObject<InstagramResponse>(textResponse);

                if (_useCache && response != null)
                {
                    _cache.Add(instagramUserCacheKey, response, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public InstagramResponse GetPostsByTag(GetPostsRequest request)
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            var instagramTagCacheKey = $"instagram_tag_{request.Query}";
            if (_useCache && _cache.Exists(instagramTagCacheKey))
            {
                return _cache.Get<InstagramResponse>(instagramTagCacheKey);
            }

            try
            {
                var textResponse = HttpUtils.Get(string.Format(TagsApiEndpoint, request.Query, _token, request.MaxCount));
                var response = JsonConvert.DeserializeObject<InstagramResponse>(textResponse);

                if (_useCache && response != null)
                {
                    _cache.Add(instagramTagCacheKey, response, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
    }
}