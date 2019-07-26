using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Geta.SocialChannels.Sample.Models.Pages
{

    /// <summary>
    /// Used for the pages to present feeds with Geta Social Channels 
    /// </summary>
    [SiteContentType(GUID = "417B5756-E112-4625-BEEE-377FF519D1FB")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class GetaSocialChannelsPage : SitePageData
    {
        public virtual string YoutubeKey { get; set; }

        public virtual string ChannelId { get; set; }

        public virtual string FacebookUserName { get; set; }

        public virtual string FacebookAppId { get; set; }

        public virtual string FacebookAppSecret { get; set; }

        public virtual string TwitterConsumerKey { get; set; }

        public virtual string TwitterSecretKey { get; set; }

        public virtual string TwitterUserName { get; set; }
    }
}