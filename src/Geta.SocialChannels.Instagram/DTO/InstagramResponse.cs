using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Geta.SocialChannels.Instagram.DTO
{
    /// <summary>
    /// These DTOs contain the properties that represent the data our custom API is interested in.
    /// </summary>
    public class CommentData
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Cursors
    {
        [JsonProperty("after")]
        public string After { get; set; }
    }

    public class Paging
    {
        [JsonProperty("cursors")]
        public Cursors Cursors { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        public Paging()
        {
            Cursors = new Cursors();
        }
    }

    public class Comments
    {
        [JsonProperty("data")]
        public List<CommentData> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        public Comments()
        {
            Paging = new Paging();
            Data = new List<CommentData>();
        }
    }

    public class MediaData
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("like_count")]
        public int LikeCount { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class HashtagSearchResult
    {
        [JsonProperty("data")]
        public List<MediaData> Data { get; set; }
    }

    public class InstagramResult
    {
        [JsonProperty("data")]
        public List<MediaData> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        public InstagramResult()
        {
            Data = new List<MediaData>();
            Paging = new Paging();
        }
    }
}
