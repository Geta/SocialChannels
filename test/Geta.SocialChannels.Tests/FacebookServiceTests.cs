using System.Linq;
using Geta.SocialChannels.Facebook;
using NUnit.Framework;

namespace Geta.SocialChannels.Tests
{
    public class FacebookServiceTests
    {
        private string _token;
        private FacebookService _facebookService;

        [SetUp]
        public void Setup()
        {
            _token = "accessToken";
            _facebookService = new FacebookService(_token);
        }

        [Test]
        public void ShouldRetrieveDataAboutUser()
        {
            var username = "getatesting";
            var accountInformation = _facebookService.GetInformation(username);
            Assert.NotNull(accountInformation);
            Assert.IsNotEmpty(accountInformation.Id);
            Assert.IsNotEmpty(accountInformation.Url);
            Assert.IsNotEmpty(accountInformation.Username);
        }
        
        [Test]
        public void ShouldThrowFacebookServiceExceptionWhenUserNotExist()
        {
            var username = "gdfgdfgdfgdfgdfgdf";
            Assert.Throws<FacebookServiceException>(() => _facebookService.GetInformation(username));
        }
        
        [Test]
        public void ShouldRetrieveUserFeed()
        {
            var request = new FacebookFeedRequest
            {
                UserName = "getatesting"
            };
            var feed = _facebookService.GetFacebookFeed(request);
            Assert.NotNull(feed);
            Assert.IsNotEmpty(feed.Data);
            var item = feed.Data.First();
            Assert.IsNotEmpty(item.Id);
            Assert.IsNotEmpty(item.CreatedTime);
            Assert.IsNotEmpty(item.Message);
        }
    }
}