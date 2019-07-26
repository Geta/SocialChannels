using System;
using System.Web.Mvc;
using EPiServer;
using Geta.SocialChannels.Facebook;
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



            model.YoutubeFeed = youtubeFeed;
            model.FacebookFeed = facebookFeed;
            model.TwitterResponse = tweets;

            return View(model);
        }

    }
}
