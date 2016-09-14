namespace Geta.SocialChannels.YouTube
{
    public class GetYoutubeFeedRequest
    {
        public string ChannelId { get; set; }

        public int MaxCount { get; set; } = 10;
    }
}