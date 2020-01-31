using Geta.SocialChannels.Instagram.Entities;
using System.Collections.Generic;

namespace Geta.SocialChannels.Instagram.Abstract
{
    public interface IInstagramService
    {
        void Config(bool useCache, int cacheDurationInMinutes);
        List<Media> GetMedia();
        List<Media> GetMediaByHashTag(string tag);
    }
}
