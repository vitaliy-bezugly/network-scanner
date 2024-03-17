using System;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Containers
{
    public class PanelContainer : IPanelContainer
    {
        public Panel Panel { get; private set; }

        public void Inialize(Panel panel)
        {
            Panel = panel ?? throw new ArgumentNullException(nameof(panel));
        }
    }
}
