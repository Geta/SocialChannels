namespace Geta.SocialChannels.YouTube
{
    public interface IYoutubeService
    {
        void Config(bool useCache, int cacheDurationInMinutes);

        GetYoutubeFeedResponse GetYoutubeFeed(GetYoutubeFeedRequest getYoutubeFeedRequest);
    }
}