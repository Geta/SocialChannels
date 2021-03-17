namespace Geta.SocialChannels.Twitter.DTO
{
    public class GetTweetsRequest
    {
        public string UserName { get; set; }

        public int MaxCount { get; set; } = 10;
    }
}