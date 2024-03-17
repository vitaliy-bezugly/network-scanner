using System;
using System.Text;
using System.Net.NetworkInformation;

namespace IpScanner.Helpers.Extensions
{
    public static class PhysicalAddressExtensions
    {
        public static Uri GetUrlToFindManufacturer(this PhysicalAddress macAddress)
        {
            string oui = macAddress.GetOui();
            return new Uri($"https://api.macvendors.com/{oui}");
        }

        public static string GetOui(this PhysicalAddress macAddress)
        {
            string oui = macAddress.ToString().Substring(0, 6);
            return oui;
        }

        public static string GetFormattedOuiOrEmptyString(this PhysicalAddress macAddress)
        {
            byte[] macBytes = macAddress.GetAddressBytes();
            if (macBytes.Length < 3)
            {
                return string.Empty;
            }

            return string.Format("{0:X2}:{1:X2}:{2:X2}", macBytes[0], macBytes[1], macBytes[2]);
        }

        public static string ToFormattedString(this PhysicalAddress macAddress)
        {
            string macString = macAddress.ToString();
            var formattedMacString = new StringBuilder();

            for (int i = 0; i < macString.Length; i++)
            {
                formattedMacString.Append(macString[i]);

                if (i % 2 == 1 && i != macString.Length - 1)
                    formattedMacString.Append(":");
            }

            return formattedMacString.ToString();
        }

        public static byte[] ConvertToBytes(this PhysicalAddress macAddress)
        {
            byte[] macBytes = new byte[6];
            string[] macParts = macAddress.ToFormattedString().Split(':');

            for (int i = 0; i < 6; i++)
            {
                macBytes[i] = Convert.ToByte(macParts[i], 16);
            }

            return macBytes;
        }
    }
}
