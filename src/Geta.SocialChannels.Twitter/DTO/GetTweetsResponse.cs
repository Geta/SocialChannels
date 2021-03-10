using System.Collections.Generic;

namespace Geta.SocialChannels.Twitter.DTO
{
    public class GetTweetsResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public List<TweetItemModel> Tweets { get; set; }
    }
}