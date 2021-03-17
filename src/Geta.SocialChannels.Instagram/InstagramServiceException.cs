using System;

namespace Geta.SocialChannels.Instagram
{
    public class InstagramServiceException : Exception
    {
        internal InstagramServiceException(string message, Exception ex)
        : base(message, ex)
        {
            
        }
    }
}