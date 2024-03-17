using OpenAI;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentResults;
using IpScanner.Services.Abstract;
using IpScanner.Helpers;

namespace IpScanner.Services
{
    internal class GptService : IGptService
    {
        private readonly IConfiguration configuration;
        private readonly AppSettings settings;

        public GptService(IConfiguration configuration, ISettingsService settingsService)
        {
            this.configuration = configuration;
            this.settings = settingsService.Settings;
        }

        public async Task<IResult<ChatCompletion>> SendPromptAsync(string prompt, CancellationToken cancellationToken)
        {
            string key = settings.GptApiKey;
            OpenAIConfiguration.ApiKey = key;

            ChatGPT3CompletionCreateOptions chatCompletionOptions = CreateOptions(prompt);
            return await TrySendPromptAsync(chatCompletionOptions, cancellationToken);
        }

        private ChatGPT3CompletionCreateOptions CreateOptions(string prompt)
        {
            string gptModel = configuration["Gpt:Model"];
            int temperature = int.Parse(configuration["Gpt:Temperature"]);

            var chatMessage = new ChatCompletionMessage
            {
                Role = ChatRoles.User,
                Content = prompt
            };

            return new ChatGPT3CompletionCreateOptions
            {
                Model = gptModel,
                Messages = new List<ChatCompletionMessage> { chatMessage },
                Temperature = temperature,
            };
        }

        private async Task<IResult<ChatCompletion>> TrySendPromptAsync(ChatGPT3CompletionCreateOptions options, CancellationToken cancellationToken)
        {
            var chatCompletionService = new ChatGPT3CompletionService();

            try
            {
                ChatCompletion chatCompletion = await chatCompletionService.CreateAsync(options, cancellationToken);
                return Result.Ok(chatCompletion);
            }
            catch (OpenAIException e)
            {
                return Result.Fail<ChatCompletion>(e.Message);
            }
            catch (TaskCanceledException)
            {
                return Result.Fail<ChatCompletion>(nameof(TaskCanceledException));
            }
        }
    }
}
