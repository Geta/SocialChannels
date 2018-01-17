using Newtonsoft.Json;

namespace Geta.SocialChannels.Instagram
{
    public class InstagramResponse
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Page { get; set; }
    }
    
    public class Datum
    {
        [JsonProperty("tags")]
        public object[] Tags { get; set; }

        [JsonProperty("created_time")]
        public double CreatedTime { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("likes")]
        public Likes Likes { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("videos")]
        public Videos Videos { get; set; }

        [JsonProperty("caption")]
        public Caption Caption { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
    
    public class Caption
    {
        [JsonProperty("created_time")]
        public double CreatedTime { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("from")]
        public PostFrom From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
    
    public class Images
    {
        [JsonProperty("standard_resolution")]
        public StandardResolutionImage Picture { get; set; }

        [JsonProperty("low_resolution")]
        public LowResolutionImage LowResPicture { get; set; }

        [JsonProperty("thumbnail")]
        public ThumbnailImage ThumbnailPicture { get; set; }
    }
    
    public class Likes
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
    
    public class LowBandwidthVideo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
    
    public class LowResolutionImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
    
    public class LowResolutionVideo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
    
    public class Pagination
    {
        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }
    }
    
    public class PostFrom
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("profile_picture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }
    
    public class StandardResolutionImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
    
    public class StandardResolutionVideo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
    
    public class ThumbnailImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
    
    public class User
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("profile_picture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }
    
    public class Videos
    {
        [JsonProperty("low_resolution")]
        public LowResolutionVideo LowResVideo { get; set; }

        [JsonProperty("standard_resolution")]
        public StandardResolutionVideo StandardResVideo { get; set; }

        [JsonProperty("low_bandwidth")]
        public LowBandwidthVideo LowBandwidthVideo { get; set; }
    }
}