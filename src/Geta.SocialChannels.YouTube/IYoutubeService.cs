using System.Collections.Generic;

namespace Geta.SocialChannels.YouTube
{
    public interface IYoutubeService
    {
        IList<YoutubeDetailModel> GetYoutubeFeed(string youtubeKey, string channelId);
    }
}