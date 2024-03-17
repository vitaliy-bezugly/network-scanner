using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
using IpScanner.Helpers;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using IpScanner.Services.Abstract;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Helpers.Messages.Ui.Enabled;
using Newtonsoft.Json.Linq;

namespace IpScanner.ViewModels.Options
{
    public partial class PeripheralPageViewModel : ObservableObject, IDisposable
    {
        [ObservableProperty]
        private bool toogleSwitchEnabled;
        [ObservableProperty]
        private bool microphoneEnabled;
        [ObservableProperty]
        private Language selectedLanguage;
        [ObservableProperty]
        private string errorText;
        [ObservableProperty]
        private string logText;
        [ObservableProperty]
        private string resultText;
        [ObservableProperty]
        private bool isListening;
        private readonly AppSettings settings;
        private readonly ISpeechRecognizerService speechRecognizerService;
        private readonly IMessenger messenger;

        public PeripheralPageViewModel(ISettingsService settingsService, 
            ISpeechRecognizerService speechRecognizerService,
            IMessenger messenger)
        {
            this.settings = settingsService.Settings;
            this.speechRecognizerService = speechRecognizerService;
            this.messenger = messenger;
            speechRecognizerService.StateChanged += SpeechRecognizer_StateChanged;

            IsListening = false;
            LogText = string.Empty;
            ErrorText = string.Empty;
            MicrophoneEnabled = settingsService.Settings.MicrophoneEnabled;
            ToogleSwitchEnabled = settingsService.Settings.MicrophoneEnabled;
            SelectedLanguage = SpeechRecognizer.SystemSpeechLanguage;
            ResultText = string.Empty;
        }
        
        public void Dispose()
        {
            speechRecognizerService.Dispose();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if(ToogleSwitchEnabled)
            {
                await EnableSpeechRecognizerAsync();
            }

            MicrophoneEnabled = ToogleSwitchEnabled;
            settings.MicrophoneEnabled = ToogleSwitchEnabled;
            messenger.Send(new MicrophoneEnabledMessage(ToogleSwitchEnabled));
        }

        [RelayCommand]
        private async Task StartRecognitionAsync()
        {
            IsListening = true;
            ErrorText = string.Empty;

            try
            {
                var enabled = await TryEnableRecognizerIfFailedNotifyUserAsync();
                if (enabled == false)
                    return;

                IResult<string> result = await speechRecognizerService.RecognizeSpeechAsync();
                if (result.IsSuccess)
                {
                    ResultText = result.Value;
                }
                else
                {
                    ToogleSwitchEnabled = false;
                    MicrophoneEnabled = false;
                    ResultText = string.Empty;
                    ErrorText = result.Errors.First().Message;

                    await SaveCommand.ExecuteAsync(this);
                }
            }
            catch (Exception exception)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog(exception.Message, "Exception");
                await messageDialog.ShowAsync();
            }
            finally
            {
                IsListening = false;
            }
        }

        private async Task<bool> TryEnableRecognizerIfFailedNotifyUserAsync()
        {
            bool enabled = await EnableSpeechRecognizerAsync();
            MicrophoneEnabled = enabled;
            settings.MicrophoneEnabled = enabled;

            return enabled;
        }

        private async Task<bool> EnableSpeechRecognizerAsync()
        {
            bool permissionGained = await speechRecognizerService.EnableAsync();

            if (permissionGained)
            {
                ErrorText = string.Empty;
            }
            else
            {
                ErrorText = "Permission to access capture resources was not given by the user; please set the application setting in Settings -> Privacy -> Microphone.";
            }

            return permissionGained;
        }

        private async void SpeechRecognizer_StateChanged(object sender, SpeechRecognizerStateChangedEventArgs args)
        {
            CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LogText = args.State.ToString();
            });
        }
    }
}
