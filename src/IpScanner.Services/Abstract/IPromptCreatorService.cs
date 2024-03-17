using IpScanner.Models.Enums;
using IpScanner.Models;
using System.Collections.Generic;

namespace IpScanner.Services.Abstract
{
    public interface IPromptCreatorService<T>
    {
        string CreatePrompt(T source, ContentFormat contentFormat);
    }
}
