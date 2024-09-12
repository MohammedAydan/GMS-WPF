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
    public partial class SubscriptionsViewModel : ObservableObject
    {
        private readonly IDataHelper<Subscription> _dataHelper;
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

        public SubscriptionsViewModel(IDataHelper<Subscription> dataHelper)
        {
            _dataHelper = dataHelper;
            // Optionally load initial data
            // LoadSubscriptionsAsync();
        }

        [ObservableProperty] private Subscription subscription = new Subscription();
        [ObservableProperty] private Subscription selectedSubscription;
        [ObservableProperty] private List<Subscription> subscriptions = new List<Subscription>();
        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string colName;
        [ObservableProperty] private string searchText;

        [RelayCommand]
        private async Task LoadSubscriptionsAsync(int page = 1)
        {
            IsLoading = true;
            try
            {
                Subscriptions = await _dataHelper.GetAllAsync(page, 50);
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
        private async Task SearchDataAsync()
        {
            IsLoading = true;
            try
            {
                var items = await _dataHelper.SearchAsync(SearchText, ColName);
                Subscriptions = new List<Subscription>(items);
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
        private async Task AddDataAsync()
        {
            ClearSubscriptionData();
            var dialog = CreateDialog("Add Subscription", "Add");
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await SaveDataAsync();
            }
        }

        [RelayCommand]
        private async Task EditDataAsync()
        {
            if (SelectedSubscription == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscription to edit.");
                return;
            }

            // Set the current subscription to the selected subscription for editing
            Subscription = SelectedSubscription;

            var dialog = CreateDialog("Edit Subscription", "Save");
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await SaveDataAsync();
            }
        }

        [RelayCommand]
        private async Task DeleteDataAsync()
        {
            if (SelectedSubscription == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscription to delete.");
                return;
            }

            var confirmationResult = await ShowConfirmationDialogAsync("Delete Subscription", "Are you sure you want to delete this subscription?");
            if (confirmationResult == ContentDialogResult.Primary)
            {
                await _dataHelper.DeleteAsync(SelectedSubscription.Id);
                await LoadSubscriptionsAsync();
            }
        }

        private void ClearSubscriptionData()
        {
            Subscription = new Subscription();
        }

        private async Task SaveDataAsync()
        {
            if (Subscription.Id != 0) // Assuming that Id 0 means new
            {
                await _dataHelper.EditAsync(Subscription);
            }
            else
            {
                await _dataHelper.AddAsync(Subscription);
            }

            await LoadSubscriptionsAsync();
        }

        private ContentDialog CreateDialog(string title, string primaryButtonText)
        {
            return new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = title,
                Content = new ManageSubscriptions(this),
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
//Wpf.Ui.Controls.