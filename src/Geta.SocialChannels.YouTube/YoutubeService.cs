using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Geta.SocialChannels.YouTube
{
    public class YoutubeService : IYoutubeService
    {
        private const string YoutubeChannelUrl =
            @"https://www.googleapis.com/youtube/v3/search?key={0}&channelId={1}&part=snippet,id&order=date&maxResults={2}";

        private const string YoutubeVideoStatisticsUrl =
            @"https://www.googleapis.com/youtube/v3/videos?part=statistics&id={0}&key={1}";

        private const string VideoLink = @"https://www.youtube.com/watch?v={0}";

        private readonly string _youtubeKey;

        private bool _useCache;
        private int _cacheDurationInMinutes = 10;

        private readonly ICache _cache;

        public YoutubeService(string youtubeKey, ICache cache)
        {
            _cache = cache;
            _youtubeKey = youtubeKey;
            _useCache = cache != null;
        }

        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            _useCache = useCache;
            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        /// <summary>
        /// Gets YouTube feed for specified channelId
        /// </summary>
        public GetYoutubeFeedResponse GetYoutubeFeed(GetYoutubeFeedRequest getYoutubeFeedRequest)
        {
            if (string.IsNullOrEmpty(this._youtubeKey) || string.IsNullOrEmpty(getYoutubeFeedRequest.ChannelId))
            {
                return null;
            }

            var key = $"youtube_cache_{getYoutubeFeedRequest.MaxCount}_{getYoutubeFeedRequest.ChannelId}";

            if (_useCache && _cache.Exists(key))
            {
                return _cache.Get<GetYoutubeFeedResponse>(key);
            }

            try
            {
                var c = new WebClient {Encoding = System.Text.Encoding.UTF8};

                var youtubeChannelUrl =
                    string.Format(YoutubeChannelUrl, _youtubeKey, getYoutubeFeedRequest.ChannelId, getYoutubeFeedRequest.MaxCount);
                var objData = c.DownloadString(youtubeChannelUrl);

                var youtubeModel = JsonConvert.DeserializeObject<YoutubeModel.RootObject>(objData);
                var youtubeDetails = CreateYoutubeDetailModels(youtubeModel, c);

                var getYoutubeFeedResponse = new GetYoutubeFeedResponse {Data = youtubeDetails};

                if (_useCache)
                {
                    _cache.Add(key, getYoutubeFeedResponse, new TimeSpan(0, _cacheDurationInMinutes, 0));
                }

                return getYoutubeFeedResponse;
            }
            catch (Exception e)
            {
                throw new YouTubeServiceException(e.Message, e);
            }
        }

        private List<YoutubeDetailModel> CreateYoutubeDetailModels(YoutubeModel.RootObject youtubeModel, WebClient c)
        {
            var youtubeDetails = new List<YoutubeDetailModel>();

            foreach (var item in youtubeModel.Items)
            {
                var videoUrlItem = item.Snippet.Thumbnails.Default.Url.Split('/').ToList();

                var videoId = videoUrlItem[4];

                var youtubeDetailUrl =
                    string.Format(YoutubeVideoStatisticsUrl, videoId, this._youtubeKey);

                var objItem = c.DownloadString(youtubeDetailUrl);

                var youtubeItemModel = Newtonsoft.Json.JsonConvert.DeserializeObject<YoutubeModel.RootObject>(objItem);

                var youtubeDetail = new YoutubeDetailModel
                {
                    Description = item.Snippet.Description,
                    ImageUrl = item.Snippet.Thumbnails.Default.Url,
                    ViewCount = youtubeItemModel.Items[0].Statistics.ViewCount,
                    LikeCount = youtubeItemModel.Items[0].Statistics.LikeCount,
                    DislikeCount = youtubeItemModel.Items[0].Statistics.DislikeCount,
                    FavoriteCount = youtubeItemModel.Items[0].Statistics.FavoriteCount,
                    PublishDate = item.Snippet.PublishedAt,
                    Title = !string.IsNullOrEmpty(item.Snippet.Title) ? item.Snippet.Title : "",
                    VideoUrl = string.Format(VideoLink, videoId)
                };

                youtubeDetails.Add(youtubeDetail);
            }

            return youtubeDetails;
        }
    }
}