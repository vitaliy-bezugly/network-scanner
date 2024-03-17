using System.IO;
using System.Linq;
using IpScanner.Models;
using System.Xml.Serialization;
using IpScanner.Domain.Contracts;
using System.Collections.Generic;
using IpScanner.Infrastructure.Mappers;
using IpScanner.Infrastructure.Entities;

namespace IpScanner.Infrastructure.ContentCreators
{
    public class DevicesXmlContentCreator : IContentCreator<Device>
    {
        public string CreateContent(IEnumerable<Device> items)
        {
            List<DeviceEntity> entities = items.Select(x => x.ToEntity()).ToList();

            var serializer = new XmlSerializer(typeof(List<DeviceEntity>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, entities);
                return writer.ToString();
            }
        }
    }
}
