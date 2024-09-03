using CommunityToolkit.Mvvm.ComponentModel; // Ensure you have this package installed
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;
using GMSMAG.Views.UserControls;
using GMSMAG.Data;
using GMSMAG.Views.Windows;
using GMSMAG.Models;

namespace GMSMAG.ViewModels.UserControls
{
    public partial class SubscribersViewModel : ObservableObject
    {
        #region Fields
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private readonly IDataHelper<Subscriber> _dataHelper;
        private readonly IDataHelper<SubscriptionsTypes> _dataHelperForSTs;
        private Subscriber _subscriber;
        private Subscriber _selectedSubscriber;
        private List<Subscriber> _subscribers;
        private string _searchText;
        private string _colName = "Id"; // Set default column name to "Id"
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private int adminId;

        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;

        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private string homePhoneNumber;

        [ObservableProperty]
        private DateTime birthday;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private List<Subscription> subscriptions = new List<Subscription>();

        [ObservableProperty]
        private DateTime? updatedAt;

        [ObservableProperty]
        private Subscriber selectedSubscriber;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string colName = "Id";

        [ObservableProperty]
        private List<Subscriber> subscribers = new List<Subscriber>();
        #endregion

        #region Commands
        [RelayCommand]
        private async Task DeleteData()
        {
            if (SelectedSubscriber == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscriber to delete.");
                return;
            }

            var res = await ShowConfirmationDialogAsync("Delete Subscriber", "Are you sure you want to delete this subscriber?");
            if (res == ContentDialogResult.Primary)
            {
                await _dataHelper.DeleteAsync(SelectedSubscriber.Id);
                await LoadData(1,50);
            }
        }

        [RelayCommand]
        private async Task SaveData()
        {
            if (SelectedSubscriber != null)
            {
                await _dataHelper.EditAsync(_subscriber);
            }
            else
            {
                await _dataHelper.AddAsync(_subscriber);
            }
            await LoadData(1,50);
        }

        [RelayCommand]
        private async Task EditData()
        {
            if (SelectedSubscriber == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscriber to edit.");
                return;
            }

            var subscriptionsTypes = await _dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Edit Subscriber",
                Content = new AddSubscriber(this, subscriptionsTypes),
                PrimaryButtonText = "Save",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
            };

            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                await SaveData();
            }
        }

        [RelayCommand]
        private async Task AddData()
        {
            ClearData();
            var subscriptionsTypes = await _dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Add Subscriber",
                Content = new AddSubscriber(this, subscriptionsTypes),
                PrimaryButtonText = "Add",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
            };

            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                await SaveData();
            }
        }
        #endregion

        // Using IAsyncRelayCommand for commands with parameters
        public IAsyncRelayCommand<int> LoadDataCommand { get; }
        public IAsyncRelayCommand SearchDataCommand { get; }

        public SubscribersViewModel()
        {
            _subscriber = new Subscriber();
            _dataHelper = App.GetService<IDataHelper<Subscriber>>();
            _dataHelperForSTs = App.GetService<IDataHelper<SubscriptionsTypes>>();

            LoadDataCommand = new AsyncRelayCommand<int>(async (page) => await LoadData(page));
            SearchDataCommand = new AsyncRelayCommand(async () => await SearchData());

            // Initial data load
            Task.Run(async () => await LoadData(1, 50));
        }

        private async Task LoadData(int page, int limit = 50)
        {
            IsLoading = true;

            try
            {
                var items = await _dataHelper.GetAllAsync(page, limit);
                Subscribers = new List<Subscriber>(items);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsLoading = false;

            }
        }

        private async Task SearchData()
        {
            var items = await _dataHelper.SearchAsync(SearchText, ColName);
            Subscribers = new List<Subscriber>(items);
        }

        private void ClearData()
        {
            AdminId = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            HomePhoneNumber = string.Empty;
            Birthday = DateTime.Now;
            Address = string.Empty;
            Subscriptions = new List<Subscription>();
            UpdatedAt = null;
        }

        private async Task ShowMessageBoxAsync(string title, string content)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Width = 300,
                Height = 150,
                Title = title,
                Content = content,
            };
            await messageBox.ShowDialogAsync();
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
                DefaultButton = ContentDialogButton.Primary,
            };

            return await dialog.ShowAsync();
        }
    }
}
