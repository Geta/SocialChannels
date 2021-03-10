using System;

namespace Geta.SocialChannels.YouTube
{
    public class YouTubeServiceException : Exception
    {
        internal YouTubeServiceException(string message, Exception e) 
            : base(message, e)
        {
            
        }
    }
}