using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Geta.SocialChannels.LinkedIn
{
    [ContentType(DisplayName = "LinkedIn Feed Block", GUID = "ec379573-c0ec-4715-95b3-c327532e0ab2", Description = "", GroupName = "SocialMedia", Order = 300)]
    public class LinkedInFeedBlock : BlockData
    {
        /// <summary>
        /// Unique string to prevent csrf.
        /// </summary>        
        [Required]
        [Display(Name = "Unique Id", Order = 10)]
        [Editable(false)]
        public virtual string Id { get; set; }

        private Guid _guid;
        /// <summary>
        /// Guid from id.
        /// </summary>
        [Ignore]
        public Guid Guid
        {
            get
            {
                if (_guid == Guid.Empty && !string.IsNullOrEmpty(Id))
                {
                    _guid = Guid.Parse(Id);
                }

                return _guid;
            }
        }

        /// <summary>
        /// LinkedIn application client id.
        /// </summary>
        [Required]
        [CultureSpecific(false)]
        [Display(Name = "Client Id", Order = 20)]
        public virtual string ClientId { get; set; }

        /// <summary>
        /// LinkedIn application client secret
        /// </summary>
        [Required]
        [CultureSpecific(false)]
        [Display(Name = "Client Secret", Order = 30)]
        public virtual string ClientSecret { get; set; }

        /// <summary>
        /// The company id.
        /// </summary>
        [Required]
        [CultureSpecific(false)]
        [Display(Name = "Company ID", Order = 40)]
        public virtual int CompanyId { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Id = Guid.NewGuid().ToString();
        }
    }
}