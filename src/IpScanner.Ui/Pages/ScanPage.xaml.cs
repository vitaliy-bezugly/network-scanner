using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Helpers.Messages.Ui;
using IpScanner.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;
using IpScanner.Services.Abstract;
using IpScanner.Services.Containers;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using System.Diagnostics;
using IpScanner.Helpers.Messages.Scanning;
using System.Linq;
using System.Collections.Generic;

namespace IpScanner.Ui.Pages
{
    public sealed partial class ScanPage : Page
    {
        private readonly IMessenger messenger;

        public ScanPage()
        {
            this.InitializeComponent();
            this.InitializePanelContainerForPrintService();

            ViewModel = Ioc.Default.GetService<ScanPageViewModel>();
            DataContext = ViewModel;

            messenger = Ioc.Default.GetService<IMessenger>();

            InitializeMessanger();
            NavigateDetailsFrameToPage();
        }

        public ScanPageViewModel ViewModel { get; }

        private void InitializePanelContainerForPrintService()
        {
            IPanelContainer panelContainer = Ioc.Default.GetService<IPanelContainer>();
            panelContainer.Inialize(CustomPrintContainer);
        }

        private void InitializeMessanger()
        {
            var messenger = Ioc.Default.GetService<IMessenger>();
            messenger.Register<SetFocusToCellMessage>(this, SetFocusToCell);
        }

        private void NavigateDetailsFrameToPage()
        {
            INavigationService navigationService = Ioc.Default.GetService<INavigationService>();
            navigationService.NavigateToPage(DetailsFrame, typeof(DetailsPage));
        }

        private void FavoritesDevicesDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.EditingElement is TextBox textbox)
            {
                textbox.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }

        private void SetFocusToCell(object sender, SetFocusToCellMessage message)
        {
            DataGrid selected = message.Favorite 
                ? FavoritesDataGrid 
                : ResultsDataGrid;

            selected.CurrentColumn = selected.Columns[(int)message.Row];
            selected.BeginEdit();
        }

        private void DevicesGrid_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            bool canApplyFile = e.DataView.Contains(StandardDataFormats.StorageItems) 
                && ViewModel.CanApplyFile();
            
            // If file is being dragged, show copy cursor. Otherwise, show no-drop cursor.
            e.AcceptedOperation = canApplyFile
                ? DataPackageOperation.Copy
                : DataPackageOperation.None;

            e.Handled = true;
        }

        private async void DevicesGrid_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            if (!e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                return;
            }

            IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
            StorageFile file = items.OfType<StorageFile>().FirstOrDefault();

            if (file != null)
            {
                await ViewModel.ApplyFileToScanContentAsync(file);
            }
        }
    }
}
