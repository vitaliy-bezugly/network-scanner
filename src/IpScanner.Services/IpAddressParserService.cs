using FluentResults;
using IpScanner.Infrastructure.ContentFormatters;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using IpScanner.Models.Enums;
using System.Collections.Generic;
using System.Net;
using Windows.Storage;
using IpScanner.Helpers.Extensions;
using System.Threading.Tasks;
using System;
using System.Linq;
using IpScanner.Models;
using IpScanner.Domain.Validators;
using IpScanner.Services.Abstract;

namespace IpScanner.Services
{
    internal class IpAddressParserService : IIpAddressParserService
    {
        private readonly IValidator<IpRange> validator;
        private readonly IContentFormatterFactory<DeviceEntity> formatterFactory;

        public IpAddressParserService(IContentFormatterFactory<DeviceEntity> formatterFactory,
            IValidator<IpRange> validator)
        {
            this.formatterFactory = formatterFactory;
            this.validator = validator;
        }

        public async Task<IResult<IEnumerable<IPAddress>>> TryParseFromFileAsync(StorageFile file)
        {
            ContentFormat contentFormat = file.FileType.ParseToContentFormat();
            IContentFormatter<DeviceEntity> formatter = formatterFactory.Create(contentFormat);

            string content = await FileIO.ReadTextAsync(file);  
            IResult<IEnumerable<DeviceEntity>> result = formatter.FormatContentAsCollection(content);

            if (result.IsSuccess)
            {
                IEnumerable<IPAddress> addresses = result.Value.Select(device => IPAddress.Parse(device.Ip));
                return Result.Ok(addresses);
            }

            return Result.Fail<IEnumerable<IPAddress>>(result.Errors);
        }

        public async Task<IResult<IEnumerable<IPAddress>>> TryParseFromRangeAsync(string range)
        {
            var ipRange = new IpRange(range);
            bool validationResult = await validator.ValidateAsync(ipRange);

            if (validationResult == false)
            {
                return Result.Fail<IEnumerable<IPAddress>>("The IP range is not valid.");
            }

            return Result.Ok(ipRange.GenerateIPAddresses());
        }

        public IEnumerable<IPAddress> ParseFromCollection(IEnumerable<Device> devices)
        {
            return devices.Select(device => device.Ip);
        }
    }
}
