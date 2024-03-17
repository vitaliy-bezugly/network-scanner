using System;
using IpScanner.Services.Abstract;
using Windows.ApplicationModel.DataTransfer;

namespace IpScanner.Services
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var dataPackage = new DataPackage();
            dataPackage.SetText(content);

            Clipboard.SetContent(dataPackage);
        }
    }
}
