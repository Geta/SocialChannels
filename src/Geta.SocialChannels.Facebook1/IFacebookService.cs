namespace Geta.SocialChannels.Facebook
{
    public interface IFacebookService
    {
        void Config(bool useCache, int cacheDurationInMinutes);
        FacbookAuthorInformation GetInformation(string userName);
        FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest);
    }
}