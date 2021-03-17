using Newtonsoft.Json;

namespace Geta.SocialChannels.Twitter.DTO
{
    public class GetAuthorizationTokenResponse
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}