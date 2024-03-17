using FluentResults;
using IpScanner.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Services.Abstract
{
    public interface IIpAddressParserService
    {
        Task<IResult<IEnumerable<IPAddress>>> TryParseFromFileAsync(StorageFile file);
        Task<IResult<IEnumerable<IPAddress>>> TryParseFromRangeAsync(string range);
        IEnumerable<IPAddress> ParseFromCollection(IEnumerable<Device> devices);
    }
}
