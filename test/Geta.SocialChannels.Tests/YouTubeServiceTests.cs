using System.Linq;
using Geta.SocialChannels.YouTube;
using NUnit.Framework;

namespace Geta.SocialChannels.Tests
{
    public class YouTubeServiceTests
    {
        private YoutubeService _youtubeService;

        [SetUp]
        public void Setup()
        {
            var key = "access_key";
            _youtubeService = new YoutubeService(key);
        }
        
        [Test]
        public void ShouldRetrieveDataAboutYouTubeFeed()
        {
            var request = new GetYoutubeFeedRequest
            {
                ChannelId = "UCLm7DMKc2OAPTihoGQwCWmw"
            };
            var ytFeed = _youtubeService.GetYoutubeFeed(request);
            Assert.IsNotNull(ytFeed);
            Assert.IsNotEmpty(ytFeed.Data);
            var media = ytFeed.Data.First();
            Assert.IsNotEmpty(media.Title);
            Assert.IsNotEmpty(media.VideoUrl);
        }
        
        [Test]
        public void ShouldThrowExceptionForWrongChannelId()
        {
            var request = new GetYoutubeFeedRequest
            {
                ChannelId = "wrong channel id"
            };
            Assert.Throws<YouTubeServiceException>(() => _youtubeService.GetYoutubeFeed(request));
        }
    }
}