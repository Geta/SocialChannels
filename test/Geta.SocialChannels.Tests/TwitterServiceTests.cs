using System.Linq;
using Geta.SocialChannels.Twitter;
using Geta.SocialChannels.Twitter.DTO;
using NUnit.Framework;

namespace Geta.SocialChannels.Tests
{
    public class TwitterServiceTests
    {
        private TwitterService _twitterService;

        [SetUp]
        public void Setup()
        {
            var apiKey = "apiKey";
            var secretKey = "secretKey";
            _twitterService = new TwitterService(apiKey, secretKey);
        }

        [Test]
        public void ShouldRetrieveDataAboutUser()
        {
            var request = new GetTweetsRequest
            {
                UserName = "microsoft"
            };
            var tweets = _twitterService.GetTweets(request);
            Assert.IsNotNull(tweets);
            Assert.IsNotEmpty(tweets);
            var tweet = tweets.First();
            Assert.IsNotEmpty(tweet.Link);
            Assert.IsNotEmpty(tweet.Text);
            Assert.IsNotEmpty(tweet.CreatedDate);
        }

        [Test]
        public void ShouldThrowTwitterExceptionForWrongApiKey()
        {
            var request = new GetTweetsRequest
            {
                UserName = "microsoft"
            };
            var twitterService = new TwitterService("apiKey", "secretKey");
            Assert.Throws<TwitterServiceException>(() => twitterService.GetTweets(request));
        }
    }
}