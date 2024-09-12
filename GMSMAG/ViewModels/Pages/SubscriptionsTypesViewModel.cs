using GMSMAG.Data;
using GMSMAG.Models;
using GMSMAG.Views.UserControls;
using GMSMAG.Views.Windows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace GMSMAG.ViewModels.Pages
{
    public partial class SubscriptionsTypesViewModel : ObservableObject
    {
        private readonly IDataHelper<SubscriptionType> _dataHelper;
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

        public SubscriptionsTypesViewModel(IDataHelper<SubscriptionType> dataHelper)
        {
            _dataHelper = dataHelper;
            // Optionally load initial data
            // Task.Run(async () => await LoadData(1));
        }

        [ObservableProperty]
        private SubscriptionType subscriptionType;

        [ObservableProperty]
        private SubscriptionType selectedSubscriptionType;

        [ObservableProperty]
        private List<SubscriptionType> subscriptionsTypes = new List<SubscriptionType>();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string colName;

        [ObservableProperty]
        private string searchText;

        [RelayCommand]
        public async Task LoadData(int page = 1)
        {
            IsLoading = true;
            try
            {
                SubscriptionsTypes = await _dataHelper.GetAllAsync(page, 50);
            }
            catch (Exception ex)
            {
                await ShowMessageBoxAsync("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task SearchData()
        {
            IsLoading = true;
            try
            {
                var items = await _dataHelper.SearchAsync(SearchText, ColName);
                SubscriptionsTypes = new List<SubscriptionType>(items);
            }
            catch (Exception ex)
            {
                await ShowMessageBoxAsync("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task ReloadData()
        {
            await LoadData();
        }

        [RelayCommand]
        public async Task EditData()
        {
            if (SelectedSubscriptionType == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscription type to edit.");
                return;
            }

            SubscriptionType = new SubscriptionType
            {
                Id = SelectedSubscriptionType.Id,
                AdminId = SelectedSubscriptionType.AdminId,
                SubscriptionName = SelectedSubscriptionType.SubscriptionName,
                Price = SelectedSubscriptionType.Price,
                SubscriptionDescription = SelectedSubscriptionType.SubscriptionDescription,
                SubscriptionFeatures = SelectedSubscriptionType.SubscriptionFeatures,
                DurationInDays = SelectedSubscriptionType.DurationInDays,
                CreatedAt = SelectedSubscriptionType.CreatedAt
            };

            var dialog = CreateDialog("Edit Subscription Type", "Save");
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await SaveData();
            }
        }

        [RelayCommand]
        public async Task AddData()
        {
            SubscriptionType = new SubscriptionType();

            var dialog = CreateDialog("Add Subscription Type", "Add");
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await SaveData();
            }
        }

        [RelayCommand]
        public async Task DeleteData()
        {
            if (SelectedSubscriptionType == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscription type to delete.");
                return;
            }

            var confirmationResult = await ShowConfirmationDialogAsync("Delete Subscription Type", "Are you sure you want to delete this subscription type?");
            if (confirmationResult == ContentDialogResult.Primary)
            {
                await _dataHelper.DeleteAsync(SelectedSubscriptionType.Id);
                await LoadData();
            }
        }

        private async Task SaveData()
        {
            if (SubscriptionType == null) return;

            if (SubscriptionType.Id != 0) // Assuming Id 0 means new
            {
                await _dataHelper.EditAsync(SubscriptionType);
            }
            else
            {
                await _dataHelper.AddAsync(SubscriptionType);
            }

            await LoadData();
        }

        private ContentDialog CreateDialog(string title, string primaryButtonText)
        {
            return new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = title,
                Content = new ManageSubscriptionType(this),
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary
            };
        }

        private async Task<ContentDialogResult> ShowConfirmationDialogAsync(string title, string content)
        {
            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                Title = title,
                Content = content,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Primary
            };

            return await dialog.ShowAsync();
        }

        private async Task ShowMessageBoxAsync(string title, string content)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Width = 300,
                Height = 150,
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };

            await messageBox.ShowDialogAsync();
        }
    }
}
