using System;
using System.Collections.Generic;

namespace Geta.SocialChannels.YouTube
{
    public class GetYoutubeFeedResponse
    {
         public List<YoutubeDetailModel> Data { get; set; } 
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
}