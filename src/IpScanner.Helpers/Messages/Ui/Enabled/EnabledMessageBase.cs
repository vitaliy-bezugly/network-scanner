namespace IpScanner.Helpers.Messages.Ui.Enabled
{
    public abstract class EnabledMessageBase
    {
        public EnabledMessageBase(bool enabled)
        {
            Enabled = enabled;
        }

        public bool Enabled { get; }
    }
}
