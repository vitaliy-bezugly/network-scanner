using IpScanner.Models.Enums;

namespace IpScanner.Infrastructure.ContentCreators.Factories
{
    public interface IContentCreatorFactory<T>
    {
        IContentCreator<T> Create(ContentFormat format);
    }
}
