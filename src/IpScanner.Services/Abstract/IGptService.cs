using FluentResults;
using OpenAI;
using System.Threading.Tasks;
using System.Threading;

namespace IpScanner.Services.Abstract
{
    public interface IGptService
    {
        Task<IResult<ChatCompletion>> SendPromptAsync(string prompt, CancellationToken cancellationToken);
    }
}
