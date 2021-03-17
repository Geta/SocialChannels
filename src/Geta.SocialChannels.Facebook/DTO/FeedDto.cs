using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Facebook.DTO
{
    public class FeedDto
    {
        [JsonProperty("data")]
        public List<FeedItemDto> Data { get; set; }
    }

    public class FeedItemDto
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("attachments")]
        public DataDto Data { get; set; }
    }

    public class DataDto
    {
        [JsonProperty("data")]
        public List<AttachmentDto> Attachments { get; set; }
    }

    public class AttachmentDto
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("media_type")]
        public string MediaType { get; set; }
    }
}