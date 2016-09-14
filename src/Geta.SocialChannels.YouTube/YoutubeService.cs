using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

        private bool _useCache = true;
        private int _cacheDurationInMinutes = 10;

        private readonly ICache _cache;

        public YoutubeService(ICache cache, string youtubeKey)
        {
            this._cache = cache;
            this._youtubeKey = youtubeKey;
        }

        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            this._useCache = useCache;
            this._cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public GetYoutubeFeedResponse GetYoutubeFeed(GetYoutubeFeedRequest getYoutubeFeedRequest)
        {
            if (string.IsNullOrEmpty(this._youtubeKey) || string.IsNullOrEmpty(getYoutubeFeedRequest.ChannelId))
            {
                return null;
            }

            var key = $"youtube_cache_{getYoutubeFeedRequest.MaxCount}_{getYoutubeFeedRequest.ChannelId}";

            if (_cache.Exists(key) && _useCache)
            {
                return _cache.Get<GetYoutubeFeedResponse>(key);
            }

            var c = new WebClient();
            c.Encoding = System.Text.Encoding.UTF8;

            var youtubeChannelUrl =
                string.Format(YoutubeChannelUrl, this._youtubeKey, getYoutubeFeedRequest.ChannelId, getYoutubeFeedRequest.MaxCount);
            var objData = c.DownloadString(youtubeChannelUrl);

            var youtubeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<YoutubeModel.RootObject>(objData);


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
                    FavoriteCount = youtubeItemModel.Items[0].Statistics.FavoriteCount,
                    PublishDate = item.Snippet.PublishedAt,
                    Title = !string.IsNullOrEmpty(item.Snippet.Title) ? item.Snippet.Title : "",
                    VideoUrl = string.Format(VideoLink, videoId)
                };

                youtubeDetails.Add(youtubeDetail);
            }

            var getYoutubeFeedResponse = new GetYoutubeFeedResponse {Data = youtubeDetails};

            if (_useCache)
            {
                _cache.Add(key, getYoutubeFeedResponse, new TimeSpan(0, _cacheDurationInMinutes, 0));
            }

            return getYoutubeFeedResponse;
        }
    }
}