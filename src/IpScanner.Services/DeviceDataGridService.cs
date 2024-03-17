using IpScanner.Models;
using IpScanner.Services.Abstract;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using IpScanner.Helpers.Extensions;
using Windows.UI.Text;

namespace IpScanner.Services
{
    internal class DeviceDataGridService : IDataGridService<Device>
    {
        public Grid CreateDataGrid(IEnumerable<Device> items)
        {
            Grid grid = InitializeGrid();
            AddColumnDefinitions(grid);

            int rowIndex = 0;
            AddHeaderRow(grid, rowIndex++);
            AddDataRows(grid, items, rowIndex);

            return grid;
        }

        private Grid InitializeGrid()
        {
            return new Grid
            {
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
        }

        private void AddColumnDefinitions(Grid grid)
        {
            var columns = new[]
            {
                GridLength.Auto, // Status
                GridLength.Auto, // Name
                GridLength.Auto, // IP
                GridLength.Auto, // Manufacturer
                GridLength.Auto, // MAC
                GridLength.Auto, // Type
                GridLength.Auto  // Comments
            };

            foreach (var column in columns)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = column });
            }
        }

        private void AddHeaderRow(Grid grid, int rowIndex)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var headers = new[] { "Status", "Name", "IP", "Manufacturer", "MAC", "Type", "Comments" };
            for (int i = 0; i < headers.Length; i++)
            {
                AddTextBlockToGrid(grid, headers[i], i, rowIndex, FontWeights.Bold);
            }
        }

        private void AddDataRows(Grid grid, IEnumerable<Device> items, int startRowIndex)
        {
            int rowIndex = startRowIndex;

            foreach (var item in items)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                AddTextBlockToGrid(grid, item.Status.ToString(), 0, rowIndex, FontWeights.Normal);
                AddTextBlockToGrid(grid, item.Name, 1, rowIndex, FontWeights.Normal);
                AddTextBlockToGrid(grid, item.Ip.ToString(), 2, rowIndex, FontWeights.Normal);
                AddTextBlockToGrid(grid, item.Manufacturer, 3, rowIndex, FontWeights.Normal);
                AddTextBlockToGrid(grid, item.MacAddress.ToFormattedString(), 4, rowIndex, FontWeights.Normal);
                AddTextBlockToGrid(grid, item.Type.ToString(), 5, rowIndex, FontWeights.Normal);
                AddTextBlockToGrid(grid, item.Comments, 6, rowIndex, FontWeights.Normal);

                rowIndex++;
            }
        }

        private void AddTextBlockToGrid(Grid grid, string text, int col, int row, FontWeight fontWeight)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Margin = new Thickness(4),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = fontWeight
            };

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Child = textBlock
            };

            Grid.SetColumn(border, col);
            Grid.SetRow(border, row);

            grid.Children.Add(border);
        }
    }
}
