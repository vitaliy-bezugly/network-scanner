using System;
using FluentResults;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;

namespace IpScanner.Services.Abstract
{
    public interface ISpeechRecognizerService : IDisposable
    {
        event EventHandler<SpeechRecognizerStateChangedEventArgs> StateChanged;
        Task<bool> EnableAsync();
        Task<IResult<string>> RecognizeSpeechAsync();
        Task ChangeLanguageAsync(Language language);
    }
}
