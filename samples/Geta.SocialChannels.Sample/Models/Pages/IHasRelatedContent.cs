using EPiServer.Core;

namespace Geta.SocialChannels.Sample.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
