using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Infrastructure.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IpScanner.Models;
using IpScanner.Helpers.Filters;
using IpScanner.Helpers;
using IpScanner.Services.Abstract;
using IpScanner.Helpers.Messages.IpRange;
using IpScanner.Helpers.Messages.Filters;
using IpScanner.Helpers.Messages.Scanning;
using IpScanner.Helpers.Messages.Ui.Visibility;
using FluentResults;
using IpScanner.Helpers.Messages.Ui.Enabled;
using IpScanner.Helpers.Constants;

namespace IpScanner.ViewModels.Bars
{
    public partial class InputBarViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool hasValidationError;
        [ObservableProperty]
        private bool ipRangeEnabled;
        [ObservableProperty]
        private ObservableCollection<string> history;
        [ObservableProperty]
        private string ipRange;
        [ObservableProperty]
        private string searchText;
        [ObservableProperty]
        private bool microphoneEnabled;
        private readonly ItemFilter<Device> searchFilter;
        private readonly AppSettings settings;
        private readonly IMessenger messenger;
        private readonly IHistoryRepository<IpRange> historyRepository;
        private readonly ISpeechRecognizerService speechRecognizerService;
        private readonly IDialogService dialogService;
        private readonly ILocalizationService localizationService;
        ISpeechSynthesizerService speechSynthesizerService;

        public InputBarViewModel(IMessenger messenger, 
            ISettingsService settingsService, 
            IHistoryRepository<IpRange> historyRepository,
            ISpeechRecognizerService speechRecognizerService,
            IDialogService dialogService,
            ILocalizationService localizationService,
            ISpeechSynthesizerService speechSynthesizerService)
        {
            this.messenger = messenger;
            this.historyRepository = historyRepository;
            this.speechRecognizerService = speechRecognizerService;
            this.dialogService = dialogService;
            this.localizationService = localizationService;
            this.speechSynthesizerService = speechSynthesizerService;
            settings = settingsService.Settings;
            history = new ObservableCollection<string>();
            searchFilter = new ItemFilter<Device>(device => device.Name.ToLower().Contains(SearchText.ToLower()));
            HasValidationError = false;
            IpRangeEnabled = settingsService.Settings.FavoritesSelected == false;
            MicrophoneEnabled = settingsService.Settings.MicrophoneEnabled;
            IpRange = NetworkHelper.GetLocalIpRange();
            RegisterMessages(messenger);
            LoadHistoryCommand.Execute(null);
        }

        partial void OnIpRangeChanging(string value)
        {
            HasValidationError = false;
        }

        partial void OnIpRangeChanged(string value)
        {
            settings.IpRange = value;
        }

        partial void OnSearchTextChanged(string value)
        {
            UpdateScannedDevicesSearchFilter();
        }

        [RelayCommand]
        private async Task LoadHistory()
        {
            var history = await historyRepository.GetItemsAsync();
            History = new ObservableCollection<string>(history.Select(x => x.Range));
        }

        [RelayCommand]
        private async Task RemoveItemFromHistoryAsync(string item)
        {
            if(string.IsNullOrEmpty(item) == true)
            {
                return;
            }

            string historyItem = History.FirstOrDefault(x => x == item);

            if (historyItem != null)
            {
                History.Remove(historyItem);
                await historyRepository.RemoveItemAsync(new IpRange(historyItem));
            }
        }

        [RelayCommand]
        private async Task RecognizeSpeechAsync()
        {
            bool permissionGained = await speechRecognizerService.EnableAsync();
            if (permissionGained)
            {
                IResult<string> result = await speechRecognizerService.RecognizeSpeechAsync();
                if (result.IsSuccess)
                {
                    string formattedResult = speechSynthesizerService.Format(result.Value);
                    IpRange = formattedResult;
                }
            }
            else
            {
                string errorTitle = localizationService.GetString(LocalizationKeys.Error);
                string errorMessage = localizationService.GetString(LocalizationKeys.MicroPermissionError);
                await dialogService.ShowMessageAsync(errorTitle, errorMessage);
            }
        }

        private void UpdateScannedDevicesSearchFilter()
        {
            messenger.Send(new ApplyFilterMessage<Device>(searchFilter, !string.IsNullOrEmpty(SearchText)));
        }

        private async void OnSaveIpRangeMessage(object sender, SaveIpRangeMessage message)
        {
            if (History.Contains(message.IpRange.Range) == false)
            {
                History.Add(message.IpRange.Range);
                await historyRepository.AddItemAsync(message.IpRange);
            }
        }

        private void OnSetSubnetMaskMessage(object sender, SetSubnetMaskMessage message)
        {
            IpRange = message.SubnetMask;
        }

        private void OnShowValidationErrorMessage(object sender, ValidationErrorMessage message)
        {
            HasValidationError = message.HasErrors;
        }

        private void OnFavoritesItemsVisibilityMessage(object sender, FavoritesItemsVisibilityMessage message)
        {
            IpRangeEnabled = message.Visible == false;
        }

        private void OnIpRangeUpdatedMessage(object sender, IpRangeUpdatedMessage message)
        {
            IpRange = message.IpRange;
        }

        private void OnMicrophoneEnabledMessage(object sender, MicrophoneEnabledMessage message)
        {
            MicrophoneEnabled = message.Enabled;
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<SaveIpRangeMessage>(this, OnSaveIpRangeMessage);
            messenger.Register<SetSubnetMaskMessage>(this, OnSetSubnetMaskMessage);
            messenger.Register<ValidationErrorMessage>(this, OnShowValidationErrorMessage);
            messenger.Register<FavoritesItemsVisibilityMessage>(this, OnFavoritesItemsVisibilityMessage);
            messenger.Register<IpRangeUpdatedMessage>(this, OnIpRangeUpdatedMessage);
            messenger.Register<MicrophoneEnabledMessage>(this, OnMicrophoneEnabledMessage);
        }
    }
}
