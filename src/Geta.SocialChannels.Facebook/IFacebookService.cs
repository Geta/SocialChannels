namespace Geta.SocialChannels.Facebook
{
    public interface IFacebookService
    {
        void Config(bool useCache, int cacheDurationInMinutes);
        FacebookAccountInformation GetInformation(string userName);
        FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest);
    }
}