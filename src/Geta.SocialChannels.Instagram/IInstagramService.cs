using System.Threading.Tasks;

namespace Geta.SocialChannels.Instagram
{
    public interface IInstagramService
    {
        void Config(bool useCache, int cacheDurationInMinutes);
        InstagramResponse GetPostsBySelf(int maxCount);
        InstagramResponse GetPostsByUser(GetPostsRequest request);
        InstagramResponse GetPostsByTag(GetPostsRequest request);
    }
}
