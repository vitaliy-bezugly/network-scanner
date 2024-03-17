using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Containers
{
    public interface IPanelContainer
    {
        Panel Panel { get; }
        void Inialize(Panel panel);
    }
}
