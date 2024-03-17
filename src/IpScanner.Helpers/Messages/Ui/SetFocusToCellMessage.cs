using IpScanner.Helpers.Enums;

namespace IpScanner.Helpers.Messages.Ui
{
    public class SetFocusToCellMessage
    {
        public SetFocusToCellMessage(DeviceRow row, bool favorite)
        {
            Row = row;
            Favorite = favorite;
        }

        public bool Favorite { get;  }
        public DeviceRow Row { get; }
    }
}
