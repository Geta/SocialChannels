namespace Geta.SocialChannels.Facebook
{
    public interface IFacebookService
    {
        void Config(string appId, string appSecret, string facebookId);

        FacbookAuthorInformation GetInformation();

        FacebookFeed GetFacebookFeed();
    }
}