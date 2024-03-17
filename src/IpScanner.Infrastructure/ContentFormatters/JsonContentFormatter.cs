using FluentResults;
using System.Text.Json;
using System.Collections.Generic;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public class JsonContentFormatter<T> : IContentFormatter<T>
    {
        public IResult<T> FormatContent(string content)
        {
            try
            {
                return Result.Ok(JsonSerializer.Deserialize<T>(content));
            }
            catch (JsonException e)
            {
                return Result.Fail<T>(new Error("The content is not in the correct format.", new Error(e.Message)));
            }
        }

        public IResult<IEnumerable<T>> FormatContentAsCollection(string content)
        {
            try
            {
                return Result.Ok(JsonSerializer.Deserialize<IEnumerable<T>>(content));
            }
            catch (JsonException e)
            {
                return Result.Fail<IEnumerable<T>>(new Error("The content is not in the correct format.", new Error(e.Message)));
            }
        }
    }
}
