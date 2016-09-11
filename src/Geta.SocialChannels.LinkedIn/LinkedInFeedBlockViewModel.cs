using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geta.SocialChannels.LinkedIn
{
    public class LinkedInFeedBlockViewModel
    {
        /// <summary>
        /// LinkedIn company id.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Indicate if the token renewal alert should be shown
        /// </summary>
        public bool ShowTokenExpiredWarning { get; set; }

        public int RemindingMinutes { get; set; }

        /// <summary>
        /// The redirect script to get access token.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// LinkedIn feeds.
        /// </summary>
        public LinkedInViewModel Feeds { get; set; }
    }

    public class LinkedInViewModel
    {
        [JsonProperty(PropertyName = "_count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "_start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "_total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "values")]
        public List<LinkedInValuesViewModel> Values { get; set; }
    }

    public class LinkedInValuesViewModel
    {
        [JsonProperty(PropertyName = "updateKey")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "updateType")]
        public string UpdateType { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }

        private DateTime? _time;

        public DateTime Time
        {
            get
            {
                if (!_time.HasValue)
                {
                    _time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Timestamp);
                }

                return _time.Value;
            }
        }

        [JsonProperty(PropertyName = "updateContent")]
        public LinkedInUpdateContentViewModel UpdateContent { get; set; }

        [JsonIgnore]
        public bool IsShowOnView =>
            UpdateContent != null
            && UpdateContent.CompanyStatusUpdate != null
            && UpdateContent.CompanyStatusUpdate.Share != null
            && (!string.IsNullOrWhiteSpace(UpdateContent.CompanyStatusUpdate.Share.Comment)
                ||
                (UpdateContent.CompanyStatusUpdate.Share.Content != null
                    && (!string.IsNullOrWhiteSpace(UpdateContent.CompanyStatusUpdate.Share.Content.Title)
                        || !string.IsNullOrWhiteSpace(UpdateContent.CompanyStatusUpdate.Share.Content.Description)))
            );

        [JsonIgnore]
        public string TextShowOnView
        {
            get
            {
                if (IsShowOnView)
                {
                    if (!string.IsNullOrWhiteSpace(UpdateContent.CompanyStatusUpdate.Share.Comment))
                    {
                        return UpdateContent.CompanyStatusUpdate.Share.Comment;
                    }

                    if (UpdateContent.CompanyStatusUpdate.Share.Content != null)
                    {
                        if (!string.IsNullOrWhiteSpace(UpdateContent.CompanyStatusUpdate.Share.Content.Title))
                        {
                            return UpdateContent.CompanyStatusUpdate.Share.Content.Title;
                        }

                        if (!string.IsNullOrWhiteSpace(UpdateContent.CompanyStatusUpdate.Share.Content.Description))
                        {
                            return UpdateContent.CompanyStatusUpdate.Share.Content.Description;
                        }
                    }
                }

                return string.Empty;
            }
        }
    }

    public class LinkedInUpdateContentViewModel
    {
        [JsonProperty(PropertyName = "company")]
        public LinkedinCompanyViewModel Company { get; set; }

        [JsonProperty(PropertyName = "companyStatusUpdate")]
        public LinkedInCompanyStatusUpdateViewModel CompanyStatusUpdate { get; set; }
    }

    public class LinkedinCompanyViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class LinkedInCompanyStatusUpdateViewModel
    {
        [JsonProperty(PropertyName = "share")]
        public LinkedInShareViewModel Share { get; set; }
    }

    public class LinkedInShareViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }

        private DateTime? _time;

        public DateTime Time
        {
            get
            {
                if (!_time.HasValue)
                {
                    _time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Timestamp);
                }

                return _time.Value;
            }
        }

        [JsonProperty(PropertyName = "content")]
        public LinkedInSharedContentViewModel Content { get; set; }
    }

    public class LinkedInSharedContentViewModel
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "eyebrowUrl")]
        public string EyebrowUrl { get; set; }

        [JsonProperty(PropertyName = "shortenedUrl")]
        public string ShortenedUrl { get; set; }

        [JsonProperty(PropertyName = "submittedImageUrl")]
        public string SubmittedImageUrl { get; set; }

        [JsonProperty(PropertyName = "submittedUrl")]
        public string SubmittedUrl { get; set; }

        [JsonProperty(PropertyName = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }

    public class LinkedInAccessTokenViewModel
    {
        /// <summary>
        /// The authorization code.
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// The access token.
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The number of seconds remaining.
        /// </summary>        
        [JsonProperty(PropertyName = "expires_in")]
        public int SecondRemaining
        {
            get
            {
                return Convert.ToInt32((ExpireTime - DateTime.Now).TotalSeconds);
            }
            set
            {
                ExpireTime = DateTime.Now.AddSeconds(value - 60);
            }
        }

        /// <summary>
        /// Expire time (UTC)
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// Check the access token is valid or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (SecondRemaining > 0 && !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(AuthorizationCode))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Flag to prevent duplicate request to linkedin from posting.
        /// </summary>
        public bool IsGettingToken { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LinkedInAccessTokenViewModel()
        {

        }

        /// <summary>
        /// Constructor with accessToken.
        /// </summary>
        /// <param name="accessTokenData"></param>
        public LinkedInAccessTokenViewModel(AccessTokenData accessTokenData)
        {
            AuthorizationCode = accessTokenData.AuthorizationCode;
            AccessToken = accessTokenData.AccessToken;
            ExpireTime = accessTokenData.ExpireTime;
            IsGettingToken = accessTokenData.IsGettingToken;
        }
    }

    public class LinkedinExceptionViewModel
    {
        /// <summary>
        /// The description of error.
        /// </summary>
        [JsonProperty(PropertyName = "error_description")]
        public string Description { get; set; }

        /// <summary>
        /// The error code.
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}