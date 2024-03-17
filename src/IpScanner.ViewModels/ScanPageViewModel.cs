using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Models.Args;
using IpScanner.Models.Enums;
using IpScanner.Domain.Validators;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using IpScanner.Models;
using IpScanner.Domain.Abstract;
using IpScanner.Helpers.Filters;
using IpScanner.Helpers.Enums;
using IpScanner.Helpers;
using IpScanner.ViewModels.Bars;
using IpScanner.ViewModels.Submenus;
using IpScanner.Services.Abstract;
using IpScanner.Helpers.Messages;
using IpScanner.Helpers.Messages.Scanning;
using IpScanner.Helpers.Messages.Progress;
using IpScanner.Helpers.Messages.Filters;
using IpScanner.Helpers.Messages.Ui.Visibility;
using IpScanner.Helpers.Messages.Printing;
using IpScanner.Helpers.Messages.IpRange;
using IpScanner.Helpers.Extensions;
using Windows.Storage;
using FluentResults;
using System.Threading.Tasks;
using IpScanner.Services;
using IpScanner.Helpers.Messages.Ui.Enabled;
using Windows.Networking.Connectivity;
using IpScanner.Helpers.Constants;

namespace IpScanner.ViewModels
{
    public partial class ScanPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool showDetails;
        [ObservableProperty]
        private Device selectedDevice;
        [ObservableProperty]
        private FilteredCollection<Device> scannedDevices;
        [ObservableProperty]
        private bool gptEnabled;
        [ObservableProperty]
        private bool reportCreating;
        private readonly string[] supportedFileTypesForReports = { ".txt", ".json" };
        private CurrentCollection currentCollection;
        private CancellationTokenSource scanningCts;
        private CancellationTokenSource reportCts;
        private readonly AppSettings settings;
        private readonly ActionBarViewModel actionBarViewModel;
        private readonly InputBarViewModel inputBarViewModel;
        private readonly FavoritesViewModel favoritesDevicesViewModel;
        private readonly ContextSubmenuViewModel contextMenuViewModel;
        private readonly IMessenger messenger;
        private readonly IDialogService dialogService;
        private readonly IPrintService<Device> printService;
        private readonly ILocalizationService localizationService;
        private readonly INetworkScanner networkScanner;
        private readonly IValidator<IpRange> ipRangeValidator;
        private readonly IIpAddressParserService ipAddressParser;
        private readonly IFileService fileService;
        private readonly IReportCreatorService<List<Device>> reportCreatorService;

        public ScanPageViewModel(IMessenger messenger,
            ILocalizationService localizationService,
            IDialogService dialogService,
            IPrintService<Device> printService,
            INetworkScanner networkScanner,
            IValidator<IpRange> ipRangeValidator,
            ISettingsService settingsService,
            IIpAddressParserService ipAddressParser,
            IFileService fileService,
            IReportCreatorService<List<Device>> reportCreatorService,
            FavoritesViewModel favoritesDevicesViewModel,
            InputBarViewModel inputBarViewModel,
            ActionBarViewModel actionBarViewModel,
            ContextSubmenuViewModel contextMenuViewModel)
        {
            settings = settingsService.Settings;
            this.messenger = messenger;
            this.printService = printService;
            this.dialogService = dialogService;
            this.localizationService = localizationService;
            this.networkScanner = networkScanner;
            this.networkScanner.DeviceScanned += DeviceScannedHandler;
            this.networkScanner.ScanningFinished += ScanningFinished;
            this.ipRangeValidator = ipRangeValidator;
            this.favoritesDevicesViewModel = favoritesDevicesViewModel;
            this.contextMenuViewModel = contextMenuViewModel;
            this.actionBarViewModel = actionBarViewModel;
            this.inputBarViewModel = inputBarViewModel;
            this.ipAddressParser = ipAddressParser;
            this.fileService = fileService;
            this.reportCreatorService = reportCreatorService;
            currentCollection = CurrentCollection.None;
            GptEnabled = settings.EnableGpt;
            ScannedDevices = new FilteredCollection<Device>();
            ReportCreating = false;

            scanningCts = new CancellationTokenSource();
            reportCts = new CancellationTokenSource();
            RegisterMessages(this.messenger);
        }

        public ActionBarViewModel ActionBarViewModel => actionBarViewModel;

        public FavoritesViewModel FavoritesDevicesViewModel => favoritesDevicesViewModel;

        public ContextSubmenuViewModel ContextMenuViewModel => contextMenuViewModel;

        public ICommand RescanCommand => actionBarViewModel.RescanCommand;

        partial void OnSelectedDeviceChanged(Device value)
        {
            messenger.Send(new DeviceSelectedMessage(value));
        }

        public async Task ApplyFileToScanContentAsync(StorageFile file)
        {
            if(file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.FileType == ".json" || file.FileType == ".xml")
            {
                var message = new StartScanningMessage<StorageFile>(ScanningSource.File, file);
                messenger.Send(message);
            }
            else
            {
                string errorTitle = localizationService.GetString(LocalizationKeys.Error);
                string errorMessage = localizationService.GetString(LocalizationKeys.NotSupportedFileType);

                await dialogService.ShowMessageAsync(errorTitle, errorMessage);
            }
        }

        public bool CanApplyFile()
        {
            return ActionBarViewModel.Scanning == false;
        }

        [RelayCommand]
        private void OnRightTapped(Device selectedItem)
        {
            SelectedDevice = selectedItem;
        }

        [RelayCommand]
        private void Clear()
        {
            ScannedDevices.Clear();
        }

        [RelayCommand]
        private async Task CreateReportAsync()
        {
            StorageFile file = await fileService.GetFileForWritingAsync(supportedFileTypesForReports);
            if(file == null)
            {
                return;
            }

            ReportCreating = true;

            List<Device> source = GetDataSource();

            ContentFormat format = file.FileType.ParseToContentFormat();
            IResult<Report<List<Device>>> report = await reportCreatorService.CreateReportAsync(source, format, reportCts.Token);

            await HandleReportResult(report, file);
            ReportCreating = false;
        }

        [RelayCommand]
        private void CancelReportCreating()
        {
            reportCts.Cancel();
            reportCts = new CancellationTokenSource();
            ReportCreating = false;
        }

        private List<Device> GetDataSource()
        {
            return FavoritesDevicesViewModel.DisplayFavorites
                ? FavoritesDevicesViewModel.FavoritesDevices.FilteredItems.ToList()
                : ScannedDevices.FilteredItems.ToList();
        }

        private async Task HandleReportResult(IResult<Report<List<Device>>> result, StorageFile file)
        {
            if (result.IsSuccess)
            {
                await fileService.WriteToFileAsync(file, result.Value.Content);
                return;
            }

            if (WasReportCreationCancelled(result))
            {
                return;
            }

            string errorMessage = result.Reasons.First().Message;
            await ShowReportErrorDialogAsync(errorMessage);
        }

        private bool WasReportCreationCancelled(IResult<Report<List<Device>>> report)
        {
            return report.Reasons.Any(x => x.Message == nameof(TaskCanceledException));
        }

        private async Task ShowReportErrorDialogAsync(string errorMessage)
        {
            string errorTitle = localizationService.GetString(LocalizationKeys.Error);
            await dialogService.ShowMessageAsync(errorTitle, errorMessage);
        }

        private async void ScanningFinished(object sender, EventArgs e)
        {
            CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => messenger.Send(new ScanningFinishedMessage()));
        }

        private async void DeviceScannedHandler(object sender, DeviceScannedEventArgs e)
        {
            CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => AddDevice(e.ScannedDevice));
        }

        private void AddDevice(Device scannedDevice)
        {
            FilteredCollection<Device> currentCollection = GetSelectedCollection();

            Device exists = currentCollection.FirstOrDefault(device => device.Ip.Equals(scannedDevice.Ip));
            if (exists != null)
            {
                currentCollection.ReplaceItem(exists, scannedDevice);
            }
            else
            {
                if (scannedDevice.Status == DeviceStatus.Online)
                {
                    currentCollection.Insert(0, scannedDevice);
                }
                else
                {
                    currentCollection.Add(scannedDevice);
                }
            }

            messenger.Send(new DeviceScannedMessage(scannedDevice));
        }

        private FilteredCollection<Device> GetSelectedCollection()
        {
            return currentCollection == CurrentCollection.Favorites
                ? FavoritesDevicesViewModel.FavoritesDevices
                : ScannedDevices;
        }

        private async void OnStartScanningAsync(object sender, StartScanningMessage<StorageFile> message)
        {
            ResetProgress();

            if (message.Source == ScanningSource.File)
            {
                await ScanFromFileAsync(message.File);
            }
            else if (message.Source == ScanningSource.IpRange)
            {
                await ScanFromIpRangeAsync();
            }
            else if (message.Source == ScanningSource.Favorites)
            {
                await RescanFavoritesAsync();
            }
        }

        private void ResetProgress()
        {
            messenger.Send(new ResetProgressMessage());
        }

        private async Task ScanFromFileAsync(StorageFile file)
        {
            currentCollection = CurrentCollection.Regular;
            IResult<IEnumerable<IPAddress>> result = await ipAddressParser.TryParseFromFileAsync(file);
            if (result.IsFailed)
            {
                await DisplayScanFromFileError();
                messenger.Send(new ScanningFinishedMessage());
                return;
            }

            ScannedDevices.Clear();
            PrepareForScanning(result.Value);
            await StartScanningAsync(result.Value);
        }

        private async Task ScanFromIpRangeAsync()
        {
            currentCollection = CurrentCollection.Regular;
            var result = await ipAddressParser.TryParseFromRangeAsync(inputBarViewModel.IpRange);

            if (result.IsFailed)
            {
                messenger.Send(new ValidationErrorMessage());
                return;
            }

            ScannedDevices.Clear();
            messenger.Send(new SaveIpRangeMessage(new IpRange(inputBarViewModel.IpRange)));

            PrepareForScanning(result.Value);
            await StartScanningAsync(result.Value);
        }

        private async Task RescanFavoritesAsync()
        {
            currentCollection = CurrentCollection.Favorites;
            List<IPAddress> addresses = ipAddressParser.ParseFromCollection(FavoritesDevicesViewModel.FavoritesDevices)
                .ToList();
            FavoritesDevicesViewModel.FavoritesDevices.Clear();

            PrepareForScanning(addresses);
            await StartScanningAsync(addresses);
        }

        private void PrepareForScanning(IEnumerable<IPAddress> items)
        {
            messenger.Send(new SetTotalCountOfScannedIpsMessage(items.Count()));
        }

        private async Task StartScanningAsync(IEnumerable<IPAddress> addresses)
        {
            // to avoid blocking UI thread in debug mode (without debugging it works fine)  
            await Task.Run(async () =>
            {
                await networkScanner.StartAsync(addresses, scanningCts.Token);
            });
        }

        private async Task DisplayScanFromFileError()
        {
            string title = localizationService.GetString(LocalizationKeys.Error);
            string content = localizationService.GetString(LocalizationKeys.FileCorrupted);

            await dialogService.ShowMessageAsync(title, content);
        }

        private void OnFilter(object sender, ApplyFilterMessage<Device> message)
        {
            ScannedDevices.ApplyFilter(message.FilterStatus, message.Filter);
        }

        private void OnDetailsPageVisibility(object sender, DetailsPageVisibilityMessage message)
        {
            ShowDetails = message.Visible;
        }

        private async void OnPrint(object sender, ShowPrintPreviewMessage message)
        {
            IEnumerable<Device> devices = FavoritesDevicesViewModel.DisplayFavorites 
                ? FavoritesDevicesViewModel.FavoritesDevices.FilteredItems
                : ScannedDevices.FilteredItems;

            await printService.ShowPrintUIAsync(devices);
        }

        private async void OnRescanSelectedItem(object sender, RescanSelectedItemMessage message)
        {
            ResetProgress();

            currentCollection = message.Source == ScanningSource.Favorites
                ? CurrentCollection.Favorites
                : CurrentCollection.Regular;

            IEnumerable<IPAddress> addresses = new List<IPAddress> { SelectedDevice.Ip };
            messenger.Send(new SetTotalCountOfScannedIpsMessage(addresses.Count()));

            await networkScanner.StartAsync(addresses, scanningCts.Token);
        }

        private void OnGptEnabledMessage(object sender, GptEnabledMessage message)
        {
            GptEnabled = message.Enabled;
        }

        private void OnUpdateScanningStatus(object sender, UpdateScanningStatusMessage message)
        {
            switch (message.Status)
            {
                case ScanningStatus.Canceled:
                    Cancel();
                    break;
                case ScanningStatus.Paused:
                    Pause();
                    break;
                case ScanningStatus.Running:
                    Resume();
                    break;
                default:
                    throw new Exception("Unknown scanning status");
            }
        }

        private void Cancel()
        {
            scanningCts.Cancel();
            ResetCancellationTokenSource();
        }

        private void ResetCancellationTokenSource()
        {
            scanningCts.Dispose();
            scanningCts = new CancellationTokenSource();
        }

        private void Pause()
        {
            networkScanner.Pause();
        }

        private void Resume()
        {
            networkScanner.Resume();
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<ApplyFilterMessage<Device>>(this, OnFilter);
            messenger.Register<DetailsPageVisibilityMessage>(this, OnDetailsPageVisibility);
            messenger.Register<ShowPrintPreviewMessage>(this, OnPrint);
            messenger.Register<StartScanningMessage<StorageFile>>(this, OnStartScanningAsync);
            messenger.Register<RescanSelectedItemMessage>(this, OnRescanSelectedItem);
            messenger.Register<UpdateScanningStatusMessage>(this, OnUpdateScanningStatus); 
            messenger.Register<GptEnabledMessage>(this, OnGptEnabledMessage);
        }
    }
}
