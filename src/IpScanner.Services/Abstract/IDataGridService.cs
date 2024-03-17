using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Abstract
{
    public interface IDataGridService<T>
    {
        Grid CreateDataGrid(IEnumerable<T> items);
    }
}
