using System;
using FluentResults;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using IpScanner.Services.Abstract;
using IpScanner.Infrastructure.Permissions;
using IpScanner.Helpers.Constants;

namespace IpScanner.Services
{
    internal class SpeechRecognizerService : ISpeechRecognizerService
    {
        private static uint HResultPrivacyStatementDeclined = 0x80045509;
        private ResourceContext speechContext;
        private ResourceMap speechResourceMap;
        private SpeechRecognizer speechRecognizer;
        private IAsyncOperation<SpeechRecognitionResult> recognitionOperation;
        private readonly ILocalizationService localizationService;

        public SpeechRecognizerService(ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
        }

        public event EventHandler<SpeechRecognizerStateChangedEventArgs> StateChanged;

        public async Task<bool> EnableAsync()
        {
            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
            if (permissionGained)
            {
                Language speechLanguage = SpeechRecognizer.SystemSpeechLanguage;

                speechContext = ResourceContext.GetForCurrentView();
                speechContext.Languages = new string[] { speechLanguage.LanguageTag };

                speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationSpeechResources");
                await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
            }

            return permissionGained;
        }

        public async Task<IResult<string>> RecognizeSpeechAsync()
        {
            try
            {
                SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeAsync();
                if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    return Result.Ok(speechRecognitionResult.Text);
                }

                string errorText = localizationService.GetString(LocalizationKeys.SpeechRecognitionFailed);
                return Result.Fail<string>(errorText + speechRecognitionResult.Status);
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException caught while recognition in progress (can be ignored):");
                System.Diagnostics.Debug.WriteLine(exception.ToString());

                string errorText = localizationService.GetString(LocalizationKeys.OperationCanceled);
                return Result.Fail<string>(errorText);
            }
            catch (Exception exception)
            {
                // Handle the speech privacy policy error.
                if ((uint)exception.HResult == HResultPrivacyStatementDeclined)
                {
                    string errorText = localizationService.GetString(LocalizationKeys.SpeechPrivacyPolicyDeclined);
                    return Result.Fail<string>(errorText);
                }

                return Result.Fail<string>(exception.Message);
            }
        }

        public async Task ChangeLanguageAsync(Language language)
        {
            if (speechRecognizer.CurrentLanguage != language)
            {
                speechContext.Languages = new string[] { language.LanguageTag };
                await InitializeRecognizer(language);
            }
        }

        public void Dispose()
        {
            if (speechRecognizer != null)
            {
                if (speechRecognizer.State != SpeechRecognizerState.Idle)
                {
                    if (recognitionOperation != null)
                    {
                        recognitionOperation.Cancel();
                        recognitionOperation = null;
                    }
                }

                speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;

                speechRecognizer.Dispose();
                speechRecognizer = null;
            }
        }

        private async Task<bool> InitializeRecognizer(Language recognizerLanguage)
        {
            if (speechRecognizer != null)
            {
                // cleanup prior to re-initializing this scenario.
                speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;

                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            // Create an instance of SpeechRecognizer.
            speechRecognizer = new SpeechRecognizer(recognizerLanguage);

            // Provide feedback to the user about the state of the recognizer.
            speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;

            // Compile the dictation topic constraint, which optimizes for dictated speech.
            var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
            speechRecognizer.Constraints.Add(dictationConstraint);
            SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

            // RecognizeWithUIAsync allows developers to customize the prompts.    
            speechRecognizer.UIOptions.AudiblePrompt = "Dictate a phrase or sentence...";
            speechRecognizer.UIOptions.ExampleText = speechResourceMap.GetValue("DictationUIOptionsExampleText", speechContext).ValueAsString;

            // Check to make sure that the constraints were in a proper format and the recognizer was able to compile it.
            return compilationResult.Status == SpeechRecognitionResultStatus.Success;
        }

        private void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }
    }
}
