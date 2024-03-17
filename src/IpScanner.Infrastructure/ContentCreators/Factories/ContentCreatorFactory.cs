using System;
using IpScanner.Models;
using IpScanner.Models.Enums;
using IpScanner.Domain.Contracts;

namespace IpScanner.Infrastructure.ContentCreators.Factories
{
    public class ContentCreatorFactory : IContentCreatorFactory<Device>
    {
        private readonly DevicesJsonContentCreator jsonContentCreator;
        private readonly DevicesXmlContentCreator xmlContentCreator;
        private readonly DevicesCsvContentCreator csvContentCreator;
        private readonly DevicesHtmlContentCreator htmlContentCreator;

        public ContentCreatorFactory(DevicesJsonContentCreator jsonContentCreator, 
            DevicesXmlContentCreator xmlContentCreator, 
            DevicesCsvContentCreator csvContentCreator, 
            DevicesHtmlContentCreator htmlContentCreator)
        {
            this.jsonContentCreator = jsonContentCreator;
            this.xmlContentCreator = xmlContentCreator;
            this.csvContentCreator = csvContentCreator;
            this.htmlContentCreator = htmlContentCreator;
        }

        public IContentCreator<Device> Create(ContentFormat format)
        {
            switch (format)
            {
                case ContentFormat.Json:
                    return jsonContentCreator;
                case ContentFormat.Xml:
                    return xmlContentCreator;
                case ContentFormat.Csv:
                    return csvContentCreator;
                case ContentFormat.Html:
                    return htmlContentCreator;
                default:
                    throw new NotImplementedException($"Content creator for format {format} is not implemented.");
            }
        }
    }
}
