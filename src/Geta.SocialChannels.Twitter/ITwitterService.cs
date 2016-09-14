namespace Geta.SocialChannels.Twitter
{
    public interface ITwitterService
    {
        void Config(bool useCache, int cacheDurationInMinutes);

        GetTweetsResponse GetTweets(GetTweetsRequest getTweetsRequest);
    }
}