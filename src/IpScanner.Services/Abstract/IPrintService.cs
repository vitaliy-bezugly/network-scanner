using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Services.Abstract
{
    public interface IPrintService<T>
    {
        Task ShowPrintUIAsync(IEnumerable<T> devicesToPrint);
    }
}
