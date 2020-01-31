using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using Geta.SocialChannels.Instagram.Abstract;
using Geta.SocialChannels.Instagram.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Media = Geta.SocialChannels.Instagram.Entities.Media;

namespace Geta.SocialChannels.Instagram
{
    [ServiceConfiguration(typeof(IInstagramService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class InstagramService: IInstagramService
    {
        private const string BaseUrl = "https://graph.facebook.com/v5.0/";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(InstagramService));
        private readonly ICache _cache;
        private readonly string _token;
        private readonly string _accountId;
        
        private bool _useCache = true;
        private int _cacheDurationInMinutes = 10;

        public InstagramService(ICache cache, string token, string accountId)
        {
            _cache = cache;
            _token = token;
            _accountId = accountId;
        }
        
        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            _useCache = useCache;
            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public List<Media> GetMedia()
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            var instagramUserCacheKey = $"IG_media_query_{_token}";
            if (_useCache && _cache.Exists(instagramUserCacheKey))
            {
                return _cache.Get<List<Media>>(instagramUserCacheKey);
            }

            try
            {
                var instagramResults = DoMediaSearch();
                var mediaModels = new List<Media>();
                foreach (var mediaData in instagramResults.media.data)
                {
                    var media = new Media
                    {
                        Id = mediaData.id,
                        LikeCount = mediaData.like_count,
                        CommentsCount = mediaData.comments_count,
                        MediaUrl = mediaData.media_url,
                        Permalink = mediaData.permalink,
                        Timestamp = mediaData.timestamp
                    };

                    mediaModels.Add(media);
                }

                if (_useCache)
                {
                    _cache.Add(instagramUserCacheKey, mediaModels, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return mediaModels;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public List<Media> GetMediaByHashTag(string tag)
        {
            if (string.IsNullOrEmpty(_token))
            {
                return null;
            }

            var instagramTagCacheKey = $"IG_hashtag_query_{tag}";
            if (_useCache && _cache.Exists(instagramTagCacheKey))
            {
                return _cache.Get<List<Media>>(instagramTagCacheKey);
            }

            try
            {
                var hashtagSearchResult = GetHashtagId(tag);
                var instagramResults = DoMediaSearchByHashtag(hashtagSearchResult.data);
                var response = instagramResults.Select(mediaData => new Media
                {
                    Id = mediaData.id,
                    LikeCount = mediaData.like_count,
                    CommentsCount = mediaData.comments_count,
                    MediaUrl = mediaData.media_url,
                    Permalink = mediaData.permalink,
                    Timestamp = mediaData.timestamp
                }).ToList();

                if (_useCache)
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

        #region Instagram Graph API Calls

        /// <summary>
        /// Get the list of media items.
        /// Parse out the response and the fields we want.
        /// Convert to DTOs and return.
        /// </summary>
        /// <returns></returns>
        private InstagramResult DoMediaSearch()
        {
            var mediaFields =
                $"{_accountId}?fields=media%7Bmedia_url%2Cmedia_type%2Ccomments_count%2Clike_count%2Ctimestamp%2Cpermalink%2Ccaption%7D";
            var mediaSearchUrl = BaseUrl + mediaFields + "&access_token=" + _token;
            var jsonResult = HttpUtils.Get(mediaSearchUrl);
            var instagramResult = JsonConvert.DeserializeObject<InstagramResult>(jsonResult);
            return instagramResult?.media != null ? instagramResult : null;
        }

        /// <summary>
        /// Get hash-tagged media.
        /// </summary>
        /// <param name="hashtagIds">Hashtag IDs</param>
        /// <returns></returns>
        private IEnumerable<MediaData> DoMediaSearchByHashtag(IEnumerable<MediaData> hashtagIds)
        {
            var result = new List<MediaData>();
            foreach (var hashtagId in hashtagIds)
            {
                var mediaSearchUrl = BaseUrl +
                                     $"{hashtagId.id}/recent_media?user_id={_accountId}&fields=id%2Cmedia_type%2Ccomments_count%2Clike_count%2Cmedia_url&access_token=" +
                                     _token;

                var jsonResult = HttpUtils.Get(mediaSearchUrl);
                var instagramResult = JsonConvert.DeserializeObject<DTO.Media>(jsonResult);
                if (instagramResult?.data != null)
                {
                    result.AddRange(instagramResult.data);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the ID of an IG Hashtag. 
        /// </summary>
        /// <param name="hashtag">The hashtag name</param>
        /// <returns>Hashtag ID</returns>
        private HashtagSearchResult GetHashtagId(string hashtag)
        {
            var mediaSearchUrl = BaseUrl + $"ig_hashtag_search?user_id={_accountId}&q={hashtag}&access_token=" + _token;
            var jsonResult = HttpUtils.Get(mediaSearchUrl);

            var instagramResult = JsonConvert.DeserializeObject<HashtagSearchResult>(jsonResult);
            return instagramResult?.data != null ? instagramResult : null;
        }

        #endregion
    }
}