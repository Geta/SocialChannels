using System;

namespace Geta.SocialChannels.Twitter
{
    public class TwitterServiceException : Exception
    {
        internal TwitterServiceException(string message, Exception e)
            : base(message, e)
        {
            
        }
    }
}