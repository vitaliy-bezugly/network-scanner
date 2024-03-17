using System.Linq;
using IpScanner.Models;
using IpScanner.Helpers.Extensions;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using System.Collections.Generic;

namespace IpScanner.Infrastructure.ContentCreators
{
    public class DevicesJsonContentCreator : IContentCreator<Device>
    {
        public string CreateContent(IEnumerable<Device> items)
        {
            if(items == null)
            {
                throw new System.ArgumentNullException(nameof(items));
            }

            List<DeviceEntity> entities = items.Select(x => x.ToEntity()).ToList();
            return entities.ToJson(true);
        }
    }
}
