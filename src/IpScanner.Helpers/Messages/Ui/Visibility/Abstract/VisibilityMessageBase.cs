namespace IpScanner.Helpers.Messages.Ui.Visibility
{
    public abstract class VisibilityMessageBase
    {
        public VisibilityMessageBase(bool isVisible)
        {
            Visible = isVisible;
        }

        public bool Visible { get; }
    }
}
