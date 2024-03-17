using System.Linq;
using IpScanner.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IpScanner.Domain.Validators
{
    public class IpRangeValidator : IValidator<IpRange>
    {
        public Task<bool> ValidateAsync(IpRange range)
        {
            string ipRange = range.Range;

            string pattern = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.(\d{1,3}-\d{1,3}|\d{1,3})(, \d{1,3}\.\d{1,3}\.\d{1,3}\.(\d{1,3}-\d{1,3}|\d{1,3}))*)$";

            return Task.FromResult(Regex.IsMatch(ipRange, pattern) && ipRange.Split(',')
                        .Select(ip => ip.Trim().Split('.'))
                        .All(parts => parts.All(ValidateIPPart)));
        }

        private bool ValidateIPPart(string part)
        {
            var ranges = part.Split('-');

            // Validate each number in the range
            if (!ranges.All(p => int.TryParse(p, out int number) && number >= 0 && number <= 255))
            {
                return false;
            }

            // If it is a range, make sure it is in increasing order
            if (ranges.Length == 2)
            {
                return int.Parse(ranges[0]) <= int.Parse(ranges[1]);
            }

            return true;
        }
    }
}
