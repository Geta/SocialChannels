using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Data.Dynamic;

namespace Geta.SocialChannels.LinkedIn
{
    public class SocialListenerController : Controller
    {
        private readonly ILinkedInService _linkedInService;

        public SocialListenerController(ILinkedInService linkedInService)
        {
            _linkedInService = linkedInService;
        }

        /// <summary>
        /// The LinkedIn listener.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Linkedin(string code, string[] state)
        {
            // Check state.
            var guid = Guid.Parse(state.FirstOrDefault());
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(AccessTokenData));
            var accessTokenData = (from at in store.Items<AccessTokenData>()
                                   where at.EntityId == guid
                                   select at).FirstOrDefault();

            if (accessTokenData == null)
            {
                return Content("The unique string for prevent csrf is invalid.");
            }

            if (!string.IsNullOrEmpty(accessTokenData.AuthorizationCode)
                && accessTokenData.AuthorizationCode.Equals(code)
                && accessTokenData.ExpireTime > DateTime.Now)
            {
                return Content("The authorization code has already been used.");
            }

            var accessTokenViewModel = await _linkedInService.ExchangeAccessTokenAsync(code, accessTokenData.ClientId, accessTokenData.ClientSecret);

            if (accessTokenViewModel != null)
            {
                // Update accessTokenData DDS.
                accessTokenData.ExpireTime = accessTokenViewModel.ExpireTime;
                accessTokenData.AccessToken = accessTokenViewModel.AccessToken;
                accessTokenData.AuthorizationCode = code;
                accessTokenData.ExpireRequestTime = DateTime.Now;
                accessTokenData.Update<AccessTokenData>();
            }
            else
            {
                return Content("Cannot exchange access token with authorization code.");
            }

            return Content("Get Linkedin access token successful.");
        }
    }
}