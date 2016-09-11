using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Geta.SocialChannels
{
    public class DynamicDataBase : IDynamicData
    {
        /// <summary>
        /// Gets or sets id of accessToken
        /// </summary>
        public Identity Id { get; set; }

        public Guid EntityId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DynamicDataBase()
        {
            var guid = Guid.NewGuid();

            Id = Identity.NewIdentity(guid);
            EntityId = guid;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public DynamicDataBase(Guid guid) : this()
        {
            Id = Identity.NewIdentity(guid);
            EntityId = guid;
        }

        /// <summary>
        /// Save the Entity.
        /// </summary>
        public void Save<T>()
        {
            var id = DynamicDataStoreFactory.Instance.GetStore(typeof(T)).Save(this);
            Id = id;
        }

        /// <summary>
        /// Update Entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Update<T>()
        {
            UpdatedDate = DateTime.Now;
            DynamicDataStoreFactory.Instance.GetStore(typeof(T)).Save(this);
        }

        /// <summary>
        /// Delete Entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Delete<T>()
        {
            DynamicDataStoreFactory.Instance.GetStore(typeof(T)).Delete(Id);
        }
    }
}