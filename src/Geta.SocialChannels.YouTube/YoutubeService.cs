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

        private readonly bool _useCache;
        private readonly int _cacheDuration;

        private readonly int _numberFeedItems;

        private readonly ICache _cache;

        public YoutubeService(ICache cache)
        {
            this._cache = cache;
            //var startPage = ContentReference.StartPage.Get<StartPage>();
            //_numberFeedItems = startPage.SocialFeedSettings.NumberSocialFeedItems > 0
            //                    ? startPage.SocialFeedSettings.NumberSocialFeedItems
            //                    : Constants.DefaultNumberSocialFeedItems;
            //_cacheDuration = startPage.SocialFeedSettings.SocialFeedCacheDuration > 0
            //                    ? startPage.SocialFeedSettings.SocialFeedCacheDuration
            //                    : Constants.DefaultSocialFeedCacheDurationForMinutes;
            //_useCache = startPage.SocialFeedSettings.EnableSocialFeedCache;
        }

        public IList<YoutubeDetailModel> GetYoutubeFeed(string youtubeKey, string channelId)
        {
            if (string.IsNullOrEmpty(youtubeKey) || string.IsNullOrEmpty(channelId))
                return null;

            var key = $"youtube_cache_{_numberFeedItems}_{channelId}";

            if (_cache.Exists(key) && _useCache)
            {
                return _cache.Get<List<YoutubeDetailModel>>(key);
            }

            var c = new WebClient();
            c.Encoding = System.Text.Encoding.UTF8;

            var youtubeChannelUrl =
                string.Format(YoutubeChannelUrl, youtubeKey, channelId, _numberFeedItems);
            var objData = c.DownloadString(youtubeChannelUrl);

            var youtubeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<YoutubeModel.RootObject>(objData);


            var youtubeDetails = new List<YoutubeDetailModel>();

            foreach (var item in youtubeModel.Items)
            {
                var videoUrlItem = item.Snippet.Thumbnails.Default.Url.Split('/').ToList();

                var videoId = videoUrlItem[4];

                var youtubeDetailUrl =
                    string.Format(YoutubeVideoStatisticsUrl, videoId, youtubeKey);

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


                if (_useCache)
                {
                    _cache.Add(key, youtubeDetails, new TimeSpan(0, _cacheDuration, 0));
                }

                youtubeDetails.Add(youtubeDetail);
            }

            return youtubeDetails;
        }
    }
}