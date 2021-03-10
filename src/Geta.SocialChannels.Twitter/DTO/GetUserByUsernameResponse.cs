using Newtonsoft.Json;

namespace Geta.SocialChannels.Twitter.DTO
{
    public class GetUserByUsernameResponse
    {
        [JsonProperty("data")]
        public User User { get; set; }
    }

    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}