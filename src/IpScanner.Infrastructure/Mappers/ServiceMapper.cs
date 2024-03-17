using System;
using IpScanner.Infrastructure.Entities;
using IpScanner.Models;

namespace IpScanner.Infrastructure.Mappers
{
    internal static class ServiceMapper
    {
        public static Service ToModel(this ServiceEntity entity)
        {
            var uri = new Uri(entity.Uri);
            return new Service(entity.Type, entity.Title, uri);
        }

        public static ServiceEntity ToEntity(this Service model)
        {
            return new ServiceEntity
            {
                Type = model.Type,
                Title = model.Title,
                Uri = model.Uri.ToString()
            };
        }
    }
}
