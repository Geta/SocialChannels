using System;
using System.Globalization;

namespace Geta.SocialChannels.Twitter
{
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