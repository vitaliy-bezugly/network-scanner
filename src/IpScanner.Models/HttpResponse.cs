using System;
using System.Linq;
using System.Collections.Generic;

namespace IpScanner.Models
{
    public class HttpResponse
    {
        private Dictionary<string, string> headers;

        public HttpResponse()
        {
            Status = string.Empty;
            Body = string.Empty;
            headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Status { get; private set; }

        public string Body { get; private set; }

        public IReadOnlyDictionary<string, string> Headers => headers;

        public string GetHeaderOrEmptyString(string name)
        {
            if (headers.TryGetValue(name, out string value))
            {
                return value;
            }

            return string.Empty;
        }

        public static HttpResponse Parse(string rawResponse)
        {
            HttpResponse response = new HttpResponse();
            var lines = rawResponse.Split(new[] { "\r\n" }, StringSplitOptions.None);

            response.Status = lines[0];

            int i;
            for (i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    break;  // Headers end with an empty line
                }

                var parts = line.Split(new[] { ": " }, 2, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    response.headers[parts[0]] = parts[1];
                }
            }

            response.Body = string.Join("\r\n", lines.Skip(i + 1));
            return response;
        }
    }
}
