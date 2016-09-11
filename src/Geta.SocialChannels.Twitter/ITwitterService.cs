using System.Collections.Generic;

namespace Geta.SocialChannels.Twitter
{
    public interface ITwitterService
    {
        void Config(string appConsumerKey, string appConsumerSecret, string userName);

        List<TweetItemModel> GetTweets();
    }
}