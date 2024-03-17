using FluentResults;
using IpScanner.Models;
using IpScanner.Models.Enums;
using IpScanner.Services.Abstract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using IpScanner.Helpers.Constants;

namespace IpScanner.Services
{
    internal class DeviceReportCreatorServiceProxy : IReportCreatorService<List<Device>>
    {
        private readonly IReportCreatorService<List<Device>> reportCreatorService;
        private readonly ILocalizationService localizationService;

        public DeviceReportCreatorServiceProxy(IReportCreatorService<List<Device>> reportCreatorService, 
            ILocalizationService localizationService)
        {
            this.reportCreatorService = reportCreatorService;
            this.localizationService = localizationService;
        }

        public async Task<IResult<Report<List<Device>>>> CreateReportAsync(List<Device> source, ContentFormat format, CancellationToken cancellationToken)
        {
            if (!IsInternetAvailable())
            {
                string noInternetMessage = localizationService.GetString(LocalizationKeys.NoInternet);
                return Result.Fail<Report<List<Device>>>(noInternetMessage);
            }

            return await reportCreatorService.CreateReportAsync(source, format, cancellationToken);
        }

        public bool IsInternetAvailable()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool available = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return available;
        }
    }
}
