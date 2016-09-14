namespace Geta.SocialChannels.Facebook
{
    public interface IFacebookService
    {
        void Config(bool useCache, int cacheDurationInMinutes);

        FacbookAuthorInformation GetInformation();

        FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest);
    }
}