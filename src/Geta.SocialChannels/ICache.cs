using System;

namespace Geta.SocialChannels
{
    public interface ICache
    {
        T Get<T>(string key);
        void Add<T>(string key, T objectToCache, TimeSpan offset);
        void Remove(string key);
        bool Exists(string key);
    }
}