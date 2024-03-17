using IpScanner.Models.Enums;
using System;

namespace IpScanner.Models
{
    public class Service
    {
        public Service(ServiceType type, string title, Uri uri)
        {
            Type = type;
            Title = title;
            Uri = uri;
        }

        public ServiceType Type { get; }
        public string Title { get; }
        public Uri Uri { get; }
    }
}
