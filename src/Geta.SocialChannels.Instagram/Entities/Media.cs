using System;
using System.Collections.Generic;

namespace Geta.SocialChannels.Instagram.Entities
{
    /// <summary>
    /// Represents a Media object from the Graph API.
    /// Messages/DTOs from the Graph API get converted to Entities as we don't
    /// need every property from the deserialized Graph API messages.
    /// </summary>
    public class Media
    {
        public string Id { get; set; }
        public string MediaUrl { get; set; }
        public string MediaType { get; set; }
        public int CommentsCount { get; set; }
        public int LikeCount { get; set; }
        public string Permalink { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime Timestamp { get; set; }

        public Media()
        {
            Comments = new List<Comment>();
        }
    }
}
