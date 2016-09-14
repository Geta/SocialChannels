namespace Geta.SocialChannels.Twitter
{
    public class GetTweetsRequest
    {
        public string UserName { get; set; }

        public int MaxCount { get; set; } = 10;
    }
}