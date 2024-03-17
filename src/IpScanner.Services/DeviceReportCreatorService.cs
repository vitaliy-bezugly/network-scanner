using FluentResults;
using IpScanner.Models;
using IpScanner.Models.Enums;
using IpScanner.Services.Abstract;
using OpenAI;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Services
{
    internal class DeviceReportCreatorService : IReportCreatorService<List<Device>>
    {
        private readonly IGptService gptService;
        private readonly IPromptCreatorService<List<Device>> promptCreatorService;

        public DeviceReportCreatorService(IGptService gptService, IPromptCreatorService<List<Device>> promptCreatorService)
        {
            this.gptService = gptService;
            this.promptCreatorService = promptCreatorService;
        }

        public async Task<IResult<Report<List<Device>>>> CreateReportAsync(List<Device> source, ContentFormat format, CancellationToken cancellationToken)
        {
            string prompt = promptCreatorService.CreatePrompt(source, format);
            IResult<ChatCompletion> result = await gptService.SendPromptAsync(prompt, cancellationToken);

            if(result.IsFailed)
            {
                return Result.Fail<Report<List<Device>>>(result.Errors);
            }

            ChatCompletion chatCompletion = result.Value;
            string content = chatCompletion.Choices.First().Message.Content;

            return Result.Ok(new Report<List<Device>>(source, content, format));
        }
    }
}
