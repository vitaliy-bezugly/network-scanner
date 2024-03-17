using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FluentResults;
using IpScanner.Models;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using IpScanner.Domain.Contracts;
using IpScanner.Helpers.Configurations;
using System.Net.NetworkInformation;
using IpScanner.Services.Abstract;
using IpScanner.Helpers;
using IpScanner.Helpers.Messages;
using IpScanner.Helpers.Messages.Toolbar;
using IpScanner.Helpers.Enums;
using IpScanner.Helpers.Constants;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.ApplicationModel;

namespace IpScanner.ViewModels.Submenus
{
    public partial class ToolsSubmenuViewModel : ObservableObject
    {
        private Device selectedDevice;
        private readonly AppSettings settings;
        private readonly ICmdService cmdService;
        private readonly IFtpService ftpService;
        private readonly IRdpService rdpService;
        private readonly IUdpService udpService;
        private readonly ITcpService tcpService;
        private readonly ITelnetService telnetService;
        private readonly IDialogService dialogService;
        private readonly IWakeOnLanService wakeOnLanService;
        private readonly IUriOpenerService uriOpenerService;
        private readonly ILocalizationService localizationService;

        public ToolsSubmenuViewModel(ICmdService cmdService,
               ITelnetService telnetService,
               IDialogService dialogService,
               ILocalizationService localizationService,
               IMessenger messenger,
               IUriOpenerService uriOpenerService,
               IFtpService ftpService,
               IRdpService rdpService,
               IWakeOnLanService wakeOnLanService,
               IUdpService udpService,
               ISettingsService settingsService,
               ITcpService tcpService)
        {
            settings = settingsService.Settings;
            this.cmdService = cmdService;
            this.telnetService = telnetService;
            this.dialogService = dialogService;
            this.localizationService = localizationService;
            this.uriOpenerService = uriOpenerService;
            this.ftpService = ftpService;
            this.rdpService = rdpService;
            this.wakeOnLanService = wakeOnLanService;
            this.udpService = udpService;
            this.tcpService = tcpService;

            RegisterMessages(messenger);
        }

        public ICommand WakeOnLanCommand => new AsyncRelayCommand(WakeOnLanAsync, CanWakeOnLan);

        [RelayCommand]
        private async Task SendUdpAsync()
        {
            if(selectedDevice == null)
            {
                return;
            }

            const int timeout = 2000;
            var udpConfiguration = new UdpConfiguration(settings.UdpPort, timeout);
            bool available = await udpService.CheckPortAvailabilityAsync(selectedDevice.Ip, udpConfiguration);

            if (available)
            {
                string title = localizationService.GetString(LocalizationKeys.Success);
                string successMessage = localizationService.GetString(LocalizationKeys.UdpSuccess);

                await dialogService.ShowMessageAsync(title, successMessage);
            }
            else
            {
                string errorTitle = localizationService.GetString(LocalizationKeys.Error);
                string errorMessage = localizationService.GetString(LocalizationKeys.UdpError);

                await dialogService.ShowMessageAsync(errorTitle, errorMessage);
            }
        }

        [RelayCommand]
        private async Task SendTcpAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            const int timeout = 2000;   
            var configuration = new TcpConfiguration(settings.TcpPort, timeout);
            bool available = await tcpService.CheckPortAvailabilityAsync(selectedDevice.Ip, configuration);

            if (available)
            {
                string title = localizationService.GetString(LocalizationKeys.Success);
                string successMessage = localizationService.GetString(LocalizationKeys.TcpSuccess);

                await dialogService.ShowMessageAsync(title, successMessage);
            }
            else
            {
                string errorTitle = localizationService.GetString(LocalizationKeys.Error);
                string errorMessage = localizationService.GetString(LocalizationKeys.TcpError);

                await dialogService.ShowMessageAsync(errorTitle, errorMessage);
            }
        }

        [RelayCommand]
        private void ResearchSsh()
        {
            if (selectedDevice == null)
            {
                return;
            }

            cmdService.Execute($"plink {selectedDevice.Ip}");
        }

        [RelayCommand]
        private async Task OpenTelnetAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            Result result = telnetService.OpenTelnetSession(selectedDevice.Ip);

            if (result.IsFailed)
            {
                string errorTitle = localizationService.GetString(LocalizationKeys.Error);
                string telnetError = localizationService.GetString(LocalizationKeys.TelnetError);

                await dialogService.ShowMessageAsync(errorTitle, telnetError);
            }
        }

        [RelayCommand]
        private async Task ExploreInExplorer()
        {
            if (selectedDevice == null)
            {
                return;
            }

            await uriOpenerService.OpenUriAsync(new Uri($"file://{selectedDevice.Ip}"));
        }

        [RelayCommand]
        private async Task ExploreHttpAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            await uriOpenerService.OpenUriAsync(new Uri($"http://{selectedDevice.Ip}"));
        }

        [RelayCommand]
        private async Task ExploreHttpsAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            await uriOpenerService.OpenUriAsync(new Uri($"https://{selectedDevice.Ip}"));
        }

        [RelayCommand]
        private async Task ExploreFtpAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            const int timeout = 2000;
            var ftpConfiguration = new FtpConfiguration($@"ftp://{selectedDevice.Ip}", "anonymous", "anonymous", timeout);
            bool connected = await ftpService.ConnectAsync(ftpConfiguration);

            if (connected)
            {
                string connectedMessage = localizationService.GetString(LocalizationKeys.Connected);
                string successfullyConnectedMessage = localizationService.GetString(LocalizationKeys.FtpSuccess);

                await dialogService.ShowMessageAsync(connectedMessage, successfullyConnectedMessage);
            }
            else
            {
                string error = localizationService.GetString(LocalizationKeys.Error);
                string couldNotConnectMessage = localizationService.GetString(LocalizationKeys.FtpError);

                await dialogService.ShowMessageAsync(error, couldNotConnectMessage);
            }
        }

        [RelayCommand]
        private async Task ExploreRdp()
        {
        }

        private async Task WakeOnLanAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            await wakeOnLanService.SendPacketAsync(selectedDevice.MacAddress, selectedDevice.Ip);
        }

        private bool CanWakeOnLan()
        {
            return selectedDevice != null && selectedDevice.MacAddress != PhysicalAddress.None;
        }

        private void OnDeviceSelected(object sender, DeviceSelectedMessage message)
        {
            selectedDevice = message.Device;
        }

        private void OnExploreSpecificItemMessage(object sender, ExploreSelectedItemMessage message)
        {
            ICommand command = GetCommandByOperationType(message.OperationType);

            if(command.CanExecute(this))
            {
                command.Execute(this);
            }
            else
            {
                string errorTitle = localizationService.GetString(LocalizationKeys.Error);
                string operationNotSupported = localizationService.GetString(LocalizationKeys.OperationNotSupported); 

                dialogService.ShowMessageAsync(errorTitle, operationNotSupported);
            }
        }

        private ICommand GetCommandByOperationType(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.FileExplorer:
                    return new AsyncRelayCommand(ExploreInExplorer);
                case OperationType.Http:
                    return new AsyncRelayCommand(ExploreHttpAsync);
                case OperationType.Https:
                    return new AsyncRelayCommand(ExploreHttpsAsync);
                case OperationType.Tcp:
                    return new AsyncRelayCommand(SendTcpAsync);
                case OperationType.Udp:
                    return new AsyncRelayCommand(SendUdpAsync);
                case OperationType.Ftp:
                    return new AsyncRelayCommand(ExploreFtpAsync);
                case OperationType.Rdp:
                    return new AsyncRelayCommand(ExploreRdp);
                case OperationType.WakeOnLan:
                    return WakeOnLanCommand;
                case OperationType.Ssh:
                    return new RelayCommand(ResearchSsh);
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(operationType), operationType, null);
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnDeviceSelected);
            messenger.Register<ExploreSelectedItemMessage>(this, OnExploreSpecificItemMessage);
        }
    }
}
