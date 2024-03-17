using IpScanner.Models.Enums;

namespace IpScanner.Models
{
    public class Report<T>
    {
        public Report(T source, string content, ContentFormat format)
        {
            Source = source;
            Content = content;
            Format = format;
        }

        public T Source { get; }
        public string Content { get; }
        public ContentFormat Format { get; }
    }
}
