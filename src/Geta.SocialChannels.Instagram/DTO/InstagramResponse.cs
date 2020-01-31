﻿using Newtonsoft.Json;
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
        public string text { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }
    }

    public class Cursors
    {
        [JsonProperty("after")]
        public string after { get; set; }
    }

    public class Paging
    {
        [JsonProperty("cursors")]
        public Cursors cursors { get; set; }

        [JsonProperty("next")]
        public string next { get; set; }

        public Paging()
        {
            this.cursors = new Cursors();
        }
    }

    public class Comments
    {
        [JsonProperty("data")]
        public List<CommentData> data { get; set; }

        [JsonProperty("paging")]
        public Paging paging { get; set; }

        public Comments()
        {
            this.paging = new Paging();
            this.data = new List<CommentData>();
        }
    }

    public class MediaData
    {
        [JsonProperty("caption")]
        public string caption { get; set; }

        [JsonProperty("media_url")]
        public string media_url { get; set; }

        [JsonProperty("media_type")]
        public string media_type { get; set; }

        [JsonProperty("comments_count")]
        public int comments_count { get; set; }

        [JsonProperty("like_count")]
        public int like_count { get; set; }

        [JsonProperty("permalink")]
        public string permalink { get; set; }

        [JsonProperty("comments")]
        public Comments comments { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime timestamp { get; set; }
    }

    public class Media
    {
        [JsonProperty("data")]
        public List<MediaData> data { get; set; }

        [JsonProperty("paging")]
        public Paging paging { get; set; }

        public Media()
        {
            this.data = new List<MediaData>();
            this.paging = new Paging();
        }
    }

    public class HashtagSearchResult
    {
        [JsonProperty("data")]
        public List<MediaData> data { get; set; }
    }

    public class InstagramResult
    {
        [JsonProperty("media")]
        public Media media { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }
    }
}
