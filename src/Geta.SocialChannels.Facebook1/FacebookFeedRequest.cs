namespace Geta.SocialChannels.Facebook
{
    public class FacebookFeedRequest
    {
        public string UserName { get; set; }
        public int MaxCount { get; set; } = 10;
    }
}