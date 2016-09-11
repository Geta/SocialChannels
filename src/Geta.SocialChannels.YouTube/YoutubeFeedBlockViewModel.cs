using System;
using System.Collections.Generic;

namespace Geta.SocialChannels.YouTube
{
    public class YoutubeFeedBlockViewModel
    {
        public string ChannelId { get; set; }

        public IList<YoutubeDetailModel> YoutubeDetailModels { get; set; }
    }

    public class YoutubeDetailModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ViewCount { get; set; }

        public string LikeCount { get; set; }

        public string DislikeCount { get; set; }

        public string FavoriteCount { get; set; }

        public DateTime PublishDate { get; set; }

        public string VideoUrl { get; set; }

        public string CreatedTimeSince => PublishDate.ToTimeSinceString();
    }

    public class YoutubeModel
    {
        public class PageInfo
        {
            public int TotalResults { get; set; }
            public int ResultsPerPage { get; set; }
        }

        public class Default
        {
            public string Url { get; set; }
        }

        public class Medium
        {
            public string Url { get; set; }
        }

        public class High
        {
            public string Url { get; set; }
        }

        public class Thumbnails
        {
            public Default Default { get; set; }
            public Medium Medium { get; set; }
            public High High { get; set; }
        }

        public class Snippet
        {
            public DateTime PublishedAt { get; set; }
            public string ChannelId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Thumbnails Thumbnails { get; set; }
            public string ChannelTitle { get; set; }
            public string LiveBroadcastContent { get; set; }
        }

        public class Item
        {
            public string Kind { get; set; }
            public string Etag { get; set; }
            //public Id id { get; set; }
            public Snippet Snippet { get; set; }
            public Statistics Statistics { get; set; }
        }

        public class RootObject
        {
            public string Kind { get; set; }

            public string Etag { get; set; }

            public PageInfo PageInfo { get; set; }

            public List<Item> Items { get; set; }

            public Statistics Statistics { get; set; }
        }

        public class Statistics
        {
            public string ViewCount { get; set; }

            public string LikeCount { get; set; }

            public string DislikeCount { get; set; }

            public string FavoriteCount { get; set; }

            public string CommentCount { get; set; }
        }
    }
}