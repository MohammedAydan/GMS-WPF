using GMSMAG.Data;
using GMSMAG.Models;
using GMSMAG.Views.UserControls;
using GMSMAG.Views.Windows;
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
            Task.Run(async () => await LoadData(1));
        }

        [ObservableProperty]
        private SubscriptionType subscriptionType;

        [ObservableProperty]
        private SubscriptionType selectedSubscriptionType;

        [ObservableProperty]
        private List<SubscriptionType> subscriptionsTypes;

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
                var res = await _dataHelper.GetAllAsync(page, 50);
                SubscriptionsTypes = res;
            }
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Edit Subscription Type",
                Content = new ManageSubscriptionType(this),
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
        public async Task AddData()
        {
            SubscriptionType = new SubscriptionType();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Add Subscription Type",
                Content = new ManageSubscriptionType(this),
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

        private async Task SaveData()
        {
            if (SubscriptionType == null) return;

            if (SelectedSubscriptionType != null)
            {
                await _dataHelper.EditAsync(SubscriptionType);
            }
            else
            {
                await _dataHelper.AddAsync(SubscriptionType);
            }

            await LoadData();
        }

        [RelayCommand]
        public async Task DeleteData()
        {
            if (SelectedSubscriptionType == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscription type to delete.");
                return;
            }

            var confirm = await ShowMessageBoxAsync("Confirmation", "Are you sure you want to delete this subscription type?");
            if (confirm == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {
                await _dataHelper.DeleteAsync(SelectedSubscriptionType.Id);
                await LoadData();
            }
        }


        private async Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageBoxAsync(string title, string content)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Width = 300,
                Height = 150,
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };

            return await messageBox.ShowDialogAsync();
        }
    }
}
