using System;
using EPiServer;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;

namespace Geta.SocialChannels
{
    /// <summary>
    /// This is a simple cache implementation to utilise EPiServer's remote cache invalidation feature on a load balanced environment
    /// Caching performed through this class will have a common dependency on the DataFactoryCache.VersionKey
    /// This means that any page publish will invalidate the entire cache store.
    /// </summary>
    [ServiceConfiguration(typeof(ICache), Lifecycle = ServiceInstanceScope.Singleton)]
    public class Cache : ICache
    {
        public T Get<T>(string key)
        {
            T cacheItem;

            try
            {
                cacheItem = (T)CacheManager.Get(key);
            }
            catch (Exception)
            {
                cacheItem = default(T);
            }

            return cacheItem;
        }


        public void Add<T>(string key, T objectToCache, TimeSpan expiration)
        {
            var cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute);
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public bool Exists(string key)
        {
            return (CacheManager.Get(key) != null);
        }

        public void Remove(string key)
        {
            CacheManager.Remove(key);
        }
    }
}