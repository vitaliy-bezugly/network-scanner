using IpScanner.Models;
using IpScanner.Domain.Validators;

namespace IpScanner.Domain.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;

    [TestClass]
    public class IpRangeValidatorUnitTests
    {
        [TestMethod]
        [DataRow("192.168.0.1-255")]
        [DataRow("192.168.0.1-155, 192.168.0.201")]
        [DataRow("192.168.0.105")]
        [DataRow("0.0.0.0")]                                // Lower bound
        [DataRow("255.255.255.255")]                        // Upper bound
        public async Task ValidateIPRange_ShouldReturnTrue_WhenValidIpRange(string range)
        {
            // Arrange
            var validator = new IpRangeValidator();
            var ipRange = new IpRange(range);

            // Act
            bool result = await validator.ValidateAsync(ipRange);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow("192.168.0.256-400")]                      // Invalid because 256 and 400 are out of range (0-255)
        [DataRow("192.168.0.155-155, 192.168.0.355")]       // Invalid because 355 is out of range
        [DataRow("192.168.0.")]                             // Invalid because it doesn't specify a host ID
        [DataRow("300.168.0.105")]                          // Invalid because 300 is out of range for an IP octet
        [DataRow("192.168.0.1-")]                           // Invalid because it doesn't specify the end of the range
        [DataRow("192.168.0.200-150")]                      // Invalid because the start is greater than the end
        [DataRow("192.168.0.155 192.168.0.201")]            // Invalid because it's missing a comma between the IP addresses
        [DataRow("")]                                       // Invalid because it's empty
        [DataRow(null)]                                     // Invalid because it's null
        [DataRow("192.168.0.1-255, ")]                      // Invalid because of trailing comma
        [DataRow(", 192.168.0.1-255")]                      // Invalid because of leading comma
        [DataRow("192.168.0.1 - 255")]                      // Invalid because of spaces around hyphen
        public async Task ValidateIPRangeShouldReturnFalseWhenInvalidIpRange(string range)
        {
            // Arrange
            var validator = new IpRangeValidator();
            var ipRange = new IpRange(range);

            // Act
            bool result = await validator.ValidateAsync(ipRange);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
