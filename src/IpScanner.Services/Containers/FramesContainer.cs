using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Containers
{
    public class FramesContainer : IFramesContainer
    {
        private readonly List<Frame> frames;

        public FramesContainer()
        {
            frames = new List<Frame>();
        }

        public void Add(Frame frame)
        {
            frames.Add(frame);
        }

        public void Remove(Frame frame)
        {
            Frame toRemove = frames.FirstOrDefault(f => f.BaseUri == frame.BaseUri);
            if (toRemove != null)
            {
                frames.Remove(toRemove);
            }
        }

        public IEnumerator<Frame> GetEnumerator()
        {
            return frames.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
