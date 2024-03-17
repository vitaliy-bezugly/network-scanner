using IpScanner.Services.Abstract;
using System.Collections.Generic;

namespace IpScanner.Services
{
    internal class SpeechSynthesizerService : ISpeechSynthesizerService
    {
        private static readonly Dictionary<string, string> FormatterDictionary;

        static SpeechSynthesizerService()
        {
            FormatterDictionary = new Dictionary<string, string>
            {
                { "dash", "-" },
                { "dot", "." },
                { "point", "." },
            };
        }

        public string Format(string input)
        {
            foreach (var (key, value) in FormatterDictionary)
            {
                input = input.Replace(key, value);
            }

            input = input.Replace(" ", "");    // Remove all spaces
            input = input.Replace(",", ", ");  // Add a space after every comma

            return input;
        }
    }
}
