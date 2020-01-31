using System.Collections.Generic;
using Geta.SocialChannels.Facebook;
using Geta.SocialChannels.Sample.Models.Pages;
using Geta.SocialChannels.Twitter;
using Geta.SocialChannels.YouTube;

namespace Geta.SocialChannels.Sample.Models.ViewModels
{
    public class GetaSocialChannelsViewModel : PageViewModel<GetaSocialChannelsPage>
    {
        public GetaSocialChannelsViewModel(GetaSocialChannelsPage currentPage) : base(currentPage)
        {
        }

        public GetYoutubeFeedResponse YoutubeFeed { get; set; }

        public FacebookFeedResponse FacebookFeed { get; set; }

        public GetTweetsResponse TwitterResponse { get; set; }

        public List<Instagram.Entities.Media> InstagramResponse { get; set; }

        public List<Instagram.Entities.Media> InstagramByTagResponse { get; set; }

        public bool ShowYoutubeFeed => YoutubeFeed?.Data != null;

        public bool ShowFacebookFeed => FacebookFeed?.Data != null;

        public bool ShowTwitterFeed => TwitterResponse?.Success == true;

        public bool ShowInstagramFeed => InstagramResponse != null;

        public bool ShowInstagramByTagFeed => InstagramByTagResponse != null;
    }
}