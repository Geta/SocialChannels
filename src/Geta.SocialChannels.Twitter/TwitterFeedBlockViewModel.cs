using System;
using System.Collections.Generic;
using System.Globalization;

namespace Geta.SocialChannels.Twitter
{
    public class TwitterFeedBlockViewModel
    {
        public virtual string TwitterAppConsumerKey { get; set; }

        public virtual string TwitterAppConsumerSecret { get; set; }

        public virtual string TwitterUserName { get; set; }

        public List<TweetItemModel> Tweets { get; set; }
    }

    public class TweetItemModel
    {
        public string StatusId { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedTimeSince
        {
            get
            {
                DateTime dateTime;

                if (!string.IsNullOrEmpty(CreatedDate) && DateTime.TryParseExact(CreatedDate, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    return dateTime.ToTimeSinceString();
                }

                return string.Empty;
            }
        }
    }
}