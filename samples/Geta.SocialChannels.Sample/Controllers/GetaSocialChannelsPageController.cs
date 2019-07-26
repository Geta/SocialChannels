using System.Web.Mvc;
using Geta.SocialChannels.Facebook;
using Geta.SocialChannels.Instagram;
using Geta.SocialChannels.LinkedIn;
using Geta.SocialChannels.Sample.Models.Pages;
using Geta.SocialChannels.Sample.Models.ViewModels;
using Geta.SocialChannels.Twitter;
using Geta.SocialChannels.YouTube;

namespace Geta.SocialChannels.Sample.Controllers
{
    public class GetaSocialChannelsPageController : PageControllerBase<GetaSocialChannelsPage>
    {
        public ActionResult Index(GetaSocialChannelsPage currentPage)
        {
            var model = new GetaSocialChannelsViewModel(currentPage);

            var youTubeService = new YoutubeService(new Cache(), currentPage.YoutubeKey);
            var youtubeFeed = youTubeService.GetYoutubeFeed(new GetYoutubeFeedRequest { ChannelId = currentPage.ChannelId });

            var facebookService = new FacebookService(new Cache(), currentPage.FacebookAppId, currentPage.FacebookAppSecret);
            var facebookFeed = facebookService.GetFacebookFeed(new FacebookFeedRequest { UserName = currentPage.FacebookUserName });

            var twitterService = new TwitterService(new Cache(), currentPage.TwitterConsumerKey, currentPage.TwitterSecretKey);
            var tweets = twitterService.GetTweets(new GetTweetsRequest {UserName = currentPage.TwitterUserName});

            var linkedInService = new LinkedInService(new Cache());
            var companyFeeds = linkedInService.GetFeedsAsync(currentPage.LinkedInAccessToken, currentPage.LinkedInCompanyId);

            var instagramService = new InstagramService(new Cache(), currentPage.InstagramAccessToken);

            var postsBySelf = instagramService.GetPostsBySelf(10);
            var postsByUser = instagramService.GetPostsByUser(new GetPostsRequest { Query = currentPage.InstagramPostsByUser, MaxCount = 10 });
            var postsByTag = instagramService.GetPostsByTag(new GetPostsRequest { Query = currentPage.InstagramPostsByTag, MaxCount = 10 });


            model.YoutubeFeed = youtubeFeed;
            model.FacebookFeed = facebookFeed;
            model.TwitterResponse = tweets;
            model.LinkedInResponse = companyFeeds;
            model.InstagramResponse = postsBySelf;
            model.InstagramResponse = postsByUser;
            model.InstagramResponse = postsByTag;

            return View(model);
        }

    }
}
