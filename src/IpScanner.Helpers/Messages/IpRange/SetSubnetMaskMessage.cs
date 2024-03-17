namespace IpScanner.Helpers.Messages.IpRange
{
    public class SetSubnetMaskMessage
    {
        public SetSubnetMaskMessage(string subnetMask)
        {
            SubnetMask = subnetMask;
        }

        public string SubnetMask { get; }
    }
}
