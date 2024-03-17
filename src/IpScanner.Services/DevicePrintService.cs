using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.Graphics.Printing;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using IpScanner.Services.Abstract;
using IpScanner.Services.Containers;
using IpScanner.Models;

namespace IpScanner.Services
{
    public class DevicePrintService : IPrintService<Device>
    {
        private PrintHelper printHelper;
        private readonly IPanelContainer panelContainer;
        private readonly IDataGridService<Device> dataGridService;

        public DevicePrintService(IPanelContainer panelContainer, IDataGridService<Device> dataGridService)
        {
            this.panelContainer = panelContainer;
            this.dataGridService = dataGridService;
        }

        public async Task ShowPrintUIAsync(IEnumerable<Device> itemsToPrint)
        {
            if(panelContainer.Panel == null)
            {
                throw new ArgumentNullException("PanelContainer.Panel is null");
            }

            printHelper = new PrintHelper(panelContainer.Panel);

            List<Device> items = itemsToPrint.ToList();
            int pages = (items.Count + 9) / 10;

            if(pages == 0)
            {
                var grid = CreatePageGrid(Enumerable.Empty<Device>(), 1);
                printHelper.AddFrameworkElementToPrint(grid);
            }
            else
            {
                for (int i = 0; i < pages; i++)
                {
                    var grid = CreatePageGrid(items.Skip(i * 10).Take(10), i + 1);
                    printHelper.AddFrameworkElementToPrint(grid);
                }
            }

            await ShowPrintDialogAsync();
        }

        private Grid CreatePageGrid(IEnumerable<Device> items, int pageNumber)
        {
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition() { Height = GridLength.Auto },
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition() { Height = GridLength.Auto }
                }
            };

            grid.Children.Add(CreateHeader());
            grid.Children.Add(dataGridService.CreateDataGrid(items));
            grid.Children.Add(CreateFooter(pageNumber));

            return grid;
        }

        private TextBlock CreateHeader() => new TextBlock { Margin = new Thickness(0, 0, 0, 20) };

        private TextBlock CreateFooter(int pageNumber)
        {
            var footer = new TextBlock { Text = $"page {pageNumber}", Margin = new Thickness(0, 20, 0, 0) };
            Grid.SetRow(footer, 2);

            return footer;
        }

        private async Task ShowPrintDialogAsync()
        {
            var printHelperOptions = new PrintHelperOptions(false)
            {
                Orientation = PrintOrientation.Landscape
            };

            printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            await printHelper.ShowPrintUIAsync("Print Sample", printHelperOptions);
        }
    }
}
