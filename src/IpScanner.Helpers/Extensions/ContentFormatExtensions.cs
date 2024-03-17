using System;
using IpScanner.Models.Enums;

namespace IpScanner.Helpers.Extensions
{
    public static class ContentFormatExtensions
    {
        public static ContentFormat ParseToContentFormat(this string fileExtension)
        {
            switch (fileExtension)
            {
                case ".json":
                    return ContentFormat.Json;
                case ".xml":
                    return ContentFormat.Xml;
                case ".csv":
                    return ContentFormat.Csv;
                case ".html":
                    return ContentFormat.Html;
                case ".txt":
                    return ContentFormat.Text;
                default:
                    throw new Exception("Unsupported file type");
            }
        }
    }
}
