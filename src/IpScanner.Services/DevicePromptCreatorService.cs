using IpScanner.Infrastructure.ContentCreators;
using IpScanner.Infrastructure.ContentCreators.Factories;
using IpScanner.Models;
using IpScanner.Models.Enums;
using IpScanner.Services.Abstract;
using System.Collections.Generic;
using System.Text;

namespace IpScanner.Services
{
    internal class DevicePromptCreatorService : IPromptCreatorService<List<Device>>
    {
        private readonly StringBuilder stringBuilder;
        private readonly IContentCreator<Device> contentCreator;

        public DevicePromptCreatorService(IContentCreatorFactory<Device> contentCreatorFactory)
        {
            stringBuilder = new StringBuilder();
            contentCreator = contentCreatorFactory.Create(ContentFormat.Json);
        }

        public string CreatePrompt(List<Device> source, ContentFormat contentFormat)
        {
            stringBuilder.AppendLine("Hello! I scanned my local network and got the following items (data provided in json format):");
            
            string json = contentCreator.CreateContent(source);
            stringBuilder.AppendLine(json);

            stringBuilder.AppendLine($"Please provide me with a report in the following format: {contentFormat}");
            stringBuilder.AppendLine("Also, analyze the size of the network and predict the nature of that network (whether it is home or public), whether it is malicious or not. Feel free to write anything that seems meaningful to you. But it is not necessary to list all available elements and describe them (this can be done only with those devices that interest you)");

            return stringBuilder.ToString();
        }
    }
}
