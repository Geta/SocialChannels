using System;
using System.Linq;
using Geta.SocialChannels.Twitter.DTO;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Twitter
{
    public class TwitterService : ITwitterService
    {
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly ICache _cache;
        private const string BaseUrl = "https://api.twitter.com/2/";
        private const string TwitterLink = "https://twitter.com/{0}/status/{1}";

        public TwitterService(string apiKey, string secretKey, ICache cache = null)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
            _cache = cache;
        }
        
        public void Config(bool useCache, int cacheDurationInMinutes)
        {
            throw new System.NotImplementedException();
        }

        public GetTweetsResponse GetTweets(GetTweetsRequest getTweetsRequest)
        {
            try
            {
                var accessToken = GetAuthorizationToken();
                var authorization =
                    $"Bearer {accessToken}";
                var getUsernameResponse = GetUserByUsername(getTweetsRequest.UserName, authorization);
                var fields = "tweet.fields=id,text,created_at"; 
                var getTweetsJsonResult = 
                    HttpUtils.Get($"{BaseUrl}users/{getUsernameResponse.User.Id}/tweets?{fields}&max_results={getTweetsRequest.MaxCount}",
                    authorization);
                var response = JsonConvert.DeserializeObject<GetUserTweetsResponse>(getTweetsJsonResult);

                return new GetTweetsResponse
                {
                    Success = true,
                    Tweets = response.Tweets.Select(s => new TweetItemModel
                    {
                        Text = s.Text,
                        CreatedDate = s.CreatedAt,
                        Link = GetTweetLink(s.Id, getUsernameResponse.User.Username)
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                return new GetTweetsResponse
                {
                    Success = false,
                    ErrorMessage = $"Error: {e.Message} Stacktrace: {e.StackTrace}"
                };
            }
            
        }

        private string GetAuthorizationToken()
        {
            var encodedApiKey = Uri.EscapeUriString(_apiKey);
            var encodedSecretKey = Uri.EscapeUriString(_secretKey);
            var base64EncodedString =  Base64Utils.Base64Encode($"{encodedApiKey}:{encodedSecretKey}");
            var contentType = "application/x-www-form-urlencoded;charset=UTF-8";
            var jsonResult = HttpUtils.Post("https://api.twitter.com/oauth2/token?grant_type=client_credentials", 
                null, $"Basic {base64EncodedString}", contentType);
            var response = JsonConvert.DeserializeObject<GetAuthorizationTokenResponse>(jsonResult);
            return response?.AccessToken;
        }
        
        private string GetTweetLink(string statusId, string username)
        {
            return string.Format(TwitterLink, username, statusId);
        }
        
        private static GetUserByUsernameResponse GetUserByUsername(string username, string authorization)
        {
            var getUserJsonResult = HttpUtils.Get($"{BaseUrl}users/by/username/{username}", authorization);
            return JsonConvert.DeserializeObject<GetUserByUsernameResponse>(getUserJsonResult);
        }
    }
}