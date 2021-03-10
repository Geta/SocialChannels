using Geta.SocialChannels.Instagram.Abstract;
using Geta.SocialChannels.Instagram.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Media = Geta.SocialChannels.Instagram.Entities.Media;

namespace Geta.SocialChannels.Instagram
{
    public class InstagramService: IInstagramService
    {
        private const string BaseUrl = "https://graph.facebook.com/v10.0/";
        private readonly ICache _cache;
        private readonly string _token;
        private readonly string _accountId;
        
        private bool _useCache;
        private int _cacheDurationInMinutes = 10;

        public InstagramService(string token, string accountId, ICache cache = null)
        {
            _cache = cache;
            _token = token;
            _accountId = accountId;
            _useCache = cache != null;
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
                var mediaList = DoMediaSearch();
                var mediaModels = new List<Media>();
                if (mediaList != null && mediaList.Any())
                {
                    foreach (var mediaData in mediaList)
                    {
                        var media = new Media
                        {
                            Id = mediaData.Id,
                            LikeCount = mediaData.LikeCount,
                            CommentsCount = mediaData.CommentsCount,
                            MediaUrl = mediaData.MediaUrl,
                            Permalink = mediaData.Permalink,
                            Timestamp = mediaData.Timestamp
                        };

                        mediaModels.Add(media);
                    }
                }

                if (_useCache)
                {
                    _cache.Add(instagramUserCacheKey, mediaModels, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return mediaModels;
            }
            catch (Exception ex)
            {
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
                var instagramResults = DoMediaSearchByHashtag(hashtagSearchResult.Data);
                var response = instagramResults.Select(mediaData => new Media
                {
                    Id = mediaData.Id,
                    LikeCount = mediaData.LikeCount,
                    CommentsCount = mediaData.CommentsCount,
                    MediaUrl = mediaData.MediaUrl,
                    Permalink = mediaData.Permalink,
                    Timestamp = mediaData.Timestamp
                }).ToList();

                if (_useCache)
                {
                    _cache.Add(instagramTagCacheKey, response, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return response;
            }
            catch (Exception ex)
            {
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
        private List<MediaData> DoMediaSearch()
        {
            var mediaFields =
                "/media?fields=id,comments_count,like_count,caption,timestamp,media_type,comments,media_url,permalink";
            var mediaSearchUrl = BaseUrl + _accountId + mediaFields + "&access_token=" + _token;
            var jsonResult = HttpUtils.Get(mediaSearchUrl);
            var media = JsonConvert.DeserializeObject<DTO.Media>(jsonResult);
            return media?.Data;
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
                var recentMediaFields = "fields=id,media_type,media_url,permalink,comments_count,like_count,timestamp";
                var mediaSearchUrl = BaseUrl +
                                     $"{hashtagId.Id}/recent_media?user_id={_accountId}&{recentMediaFields}&access_token=" +
                                     _token;

                var jsonResult = HttpUtils.Get(mediaSearchUrl);
                var instagramResult = JsonConvert.DeserializeObject<DTO.Media>(jsonResult);
                if (instagramResult?.Data != null)
                {
                    result.AddRange(instagramResult.Data);
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
            return instagramResult?.Data != null ? instagramResult : null;
        }

        #endregion
    }
}