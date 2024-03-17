using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using IpScanner.Helpers.Extensions;
using IpScanner.Infrastructure.Repositories.Abstract;

namespace IpScanner.Infrastructure.Repositories
{
    internal class FeatureRepository : IFeatureRepository
    {
        private readonly Dictionary<string, long> statusEncoder;
        private readonly Dictionary<string, long> ouiEncoder;
        private readonly Dictionary<string, long> manufacturerEncoder;

        public FeatureRepository(IConfiguration configuration)
        {
            string statusMappingPath = configuration["MachineLearning:StatusMappingPath"];
            statusEncoder = File.ReadAllText(statusMappingPath).FromJson<Dictionary<string, long>>();

            string ouiMappingPath = configuration["MachineLearning:OuiMappingPath"];
            ouiEncoder = File.ReadAllText(ouiMappingPath).FromJson<Dictionary<string, long>>();

            string manufacturerMappingPath = configuration["MachineLearning:ManufacturerMappingPath"];
            manufacturerEncoder = File.ReadAllText(manufacturerMappingPath).FromJson<Dictionary<string, long>>();
        }

        public long GetStatusValueOrDefault(string status)
        {
            return statusEncoder.ContainsKey(status) ? statusEncoder[status] : -1;
        }

        public long GetOuiValueOrDefault(string oui)
        {
            return ouiEncoder.ContainsKey(oui) ? ouiEncoder[oui] : -1;
        }

        public long GetManufacturerValueOrDefault(string manufacturer)
        {
            return manufacturerEncoder.ContainsKey(manufacturer) ? manufacturerEncoder[manufacturer] : -1;
        }
    }
}
