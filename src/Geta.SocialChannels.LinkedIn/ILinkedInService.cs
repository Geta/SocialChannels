using System;
using System.Threading.Tasks;

namespace Geta.SocialChannels.LinkedIn
{
    public interface ILinkedInService
    {
        /// <summary>
        /// Get access token from DDS.
        /// </summary>
        LinkedInAccessTokenViewModel GetAccessTokenData(LinkedInFeedBlock linkedInFeedBlock);

        /// <summary>
        /// Get authorization url.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="guid">The unique string to prevent csrf.</param>
        /// <returns></returns>
        Uri GetAuthorizationUrl(string clientId, Guid guid);

        /// <summary>
        /// Exchange authorization code for access token.
        /// </summary>
        Task<LinkedInAccessTokenViewModel> ExchangeAccessTokenAsync(string authorizationCode, string clientId, string clientSecet);

        /// <summary>
        /// Get linkedin company feeds.
        /// </summary>
        /// <returns></returns>
        Task<LinkedInViewModel> GetFeedsAsync(string accessToken, string companyId);
    }
}