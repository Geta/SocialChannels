using System.Collections.Generic;
using Geta.SocialChannels.Twitter.DTO;

namespace Geta.SocialChannels.Twitter
{
    public interface ITwitterService
    {
        void Config(bool useCache, int cacheDurationInMinutes);

        List<TweetItemModel> GetTweets(GetTweetsRequest getTweetsRequest);
    }
}