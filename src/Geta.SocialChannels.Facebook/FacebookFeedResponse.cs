using System;
using System.Collections.Generic;

namespace Geta.SocialChannels.Facebook
{
    public class FacebookFeedResponse
    {
        public List<FacebookPostItem> Data;
    }

    public class FacebookAccountInformation
    {
        public string Id { get; set; }
        public string Url => $"https://www.facebook.com/{Id}";
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Location Location { get; set; }
        public string Username { get; set; }
    }

    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
    }

    public class FacebookPostItem
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string CreatedTime { get; set; }
        public List<FacebookAttachment> Attachments { get; set; }
    }

    public class FacebookAttachment
    {
        public string MediaType { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}