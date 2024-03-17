using System.IO;
using CsvHelper;
using System.Globalization;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using IpScanner.Domain.Contracts;
using System.Net.NetworkInformation;
using IpScanner.Infrastructure.Entities;
using IpScanner.Helpers.Extensions;
using Microsoft.Extensions.Configuration;

namespace IpScanner.Infrastructure.Repositories
{
    public class ManufacturerCsvRepository : IManufactorRepository
    {
        private readonly string filePath;

        public ManufacturerCsvRepository(IConfiguration configuration)
        {
            filePath = configuration["Manufacturer:FileName"];
        }

        public async Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress)
        {
            return await Task.Run(() => FindManufacturerByAssignment(macAddress.GetOui()));
        }

        private string FindManufacturerByAssignment(string assignment)
        {
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return SearchCsvForAssignment(csv, assignment);
                }
            }
        }

        private string SearchCsvForAssignment(CsvReader csv, string assignment)
        {
            while (csv.Read())
            {
                var record = csv.GetRecord<MacAddressEntity>();
                if (record.Assignment == assignment)
                {
                    return record.OrganizationName;
                }
            }

            return string.Empty;
        }
    }
}
