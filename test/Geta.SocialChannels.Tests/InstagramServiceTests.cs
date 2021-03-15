using System.Linq;
using Geta.SocialChannels.Instagram;
using NUnit.Framework;

namespace Geta.SocialChannels.Tests
{
    public class InstagramServiceTests
    {
        private string _accountId;
        private string _token;
        private InstagramService _instagramService;

        [SetUp]
        public void Setup()
        {
            // Use account at least one photo/video
            _accountId = "accountId";
            _token = "accessToken";
            _instagramService = new InstagramService(_token, _accountId);
        }

        [Test]
        public void ShouldRetrieveDataForOsloHashtag()
        {
            var hashtagData = _instagramService.GetMediaByHashTag("oslo");
            Assert.NotNull(hashtagData);
            Assert.IsNotEmpty(hashtagData);
            var media = hashtagData.First();
            Assert.IsNotEmpty(media.Id);
            Assert.IsNotEmpty(media.MediaUrl);
            Assert.IsNotEmpty(media.Permalink);
        }
        
        [Test]
        public void ShouldRetrieveUserMedia()
        {
            var hashtagData = _instagramService.GetMedia();
            Assert.NotNull(hashtagData);
            Assert.IsNotEmpty(hashtagData);
            var media = hashtagData.First();
            Assert.IsNotEmpty(media.Id);
            Assert.IsNotEmpty(media.MediaUrl);
            Assert.IsNotEmpty(media.Permalink);
        }
        
        [Test]
        public void ShouldThrowInstagramServiceExceptionWhenAccountIdIsWrong()
        {
            var instagramService = new InstagramService(_token, "test");
            Assert.Throws<InstagramServiceException>(() => instagramService.GetMedia());
        }
    }
}