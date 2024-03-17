using IpScanner.Helpers.Enums;

namespace IpScanner.Helpers.Messages.Toolbar
{
    public class ExploreSelectedItemMessage
    {
        public ExploreSelectedItemMessage(OperationType operationType)
        {
            OperationType = operationType;
        }

        public OperationType OperationType { get; }
    }
}
