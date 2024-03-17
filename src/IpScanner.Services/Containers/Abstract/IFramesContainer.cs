using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Containers
{
    public interface IFramesContainer : IEnumerable<Frame>
    {
        void Add(Frame frame);
        void Remove(Frame frame);
    }
}
