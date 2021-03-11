using System;

namespace Geta.SocialChannels.Facebook
{
    public class FacebookServiceException : Exception
    {
        internal FacebookServiceException(string message, Exception e)
            : base(message, e)
        {
            
        }
    }
}