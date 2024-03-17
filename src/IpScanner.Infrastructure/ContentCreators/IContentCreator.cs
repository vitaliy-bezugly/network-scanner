using System.Collections.Generic;

namespace IpScanner.Infrastructure.ContentCreators
{
    public interface IContentCreator<T>
    {
        string CreateContent(IEnumerable<T> items);
    }
}
