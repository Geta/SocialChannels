using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Twitter.DTO
{
    public class GetUserTweetsResponse
    {
        [JsonProperty("data")]
        public List<TweetDto> Tweets { get; set; }
    }

    public class TweetDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}