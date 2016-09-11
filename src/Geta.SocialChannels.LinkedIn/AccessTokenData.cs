using System;
using EPiServer.Data.Dynamic;

namespace Geta.SocialChannels.LinkedIn
{
    [EPiServerDataStore(AutomaticallyRemapStore = true, AutomaticallyCreateStore = true)]
    public class AccessTokenData : DynamicDataBase
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public int CompanyId { get; set; }

        public string AuthorizationCode { get; set; }

        public string AccessToken { get; set; }

        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// Flag to prevent duplicate requets to linkedin.
        /// </summary>
        public bool IsGettingToken => ExpireRequestTime > DateTime.Now;

        /// <summary>
        /// Previous request time.
        /// </summary>
        public DateTime ExpireRequestTime { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AccessTokenData()
        {
        }

        /// <summary>
        /// Constructor with guid, code, accesstoken
        /// </summary>
        public AccessTokenData(LinkedInFeedBlock linkedInFeedBlock, string authorizationCode, string accessToken, DateTime expireTime) : base(linkedInFeedBlock.Guid)
        {
            ClientId = linkedInFeedBlock.ClientId;
            ClientSecret = linkedInFeedBlock.ClientSecret;
            CompanyId = linkedInFeedBlock.CompanyId;
            AuthorizationCode = authorizationCode;
            AccessToken = accessToken;
            ExpireTime = expireTime;
            ExpireRequestTime = DateTime.Now.AddMinutes(-1);
        }

        /// <summary>
        /// Reset access token.
        /// </summary>
        public void ResetAccessToken()
        {
            ExpireTime = DateTime.Now.AddMinutes(-1);
            ExpireRequestTime = DateTime.Now.AddMinutes(-1);
        }
    }
}