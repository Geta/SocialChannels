using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Facebook
{
    public class FacebookFeedResponse
    {
        public List<FacebookPostItem> Data;
    }

    public class FacbookAuthorInformation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url => $"https://www.facebook.com/{Id}";
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
}