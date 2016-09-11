using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Facebook
{
    public class FacebookFeedBlockViewModel
    {
        public string FacebookAppId { get; set; }

        public FacbookAuthorInformation About;

        public FacebookFeed FeedData { get; set; }
    }

    public class FacebookFeed
    {
        public List<FacebookPostItem> Data;

        public FacebookPagingModel Paging;
    }

    public class FacbookAuthorInformation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url => string.Format("https://www.facebook.com/{0}", Id);
    }

    public class FacebookPostItem
    {
        public string Id { get; set; }

        public string Message { get; set; }

        [JsonProperty(PropertyName = "Created_Time")]
        public DateTime CreatedTime { get; set; }

        public string CreatedTimeSince => CreatedTime.ToTimeSinceString();

        public FacbookAuthorInformation From { get; set; }
    }

    public class FacebookPagingModel
    {
        public string Next { get; set; }

        public string Previous { get; set; }
    }
}