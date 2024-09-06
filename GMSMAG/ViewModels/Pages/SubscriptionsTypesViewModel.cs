using GMSMAG.Data;
using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.ViewModels.Pages
{
    public partial class SubscriptionsTypesViewModel :ObservableObject
    {
        private readonly IDataHelper<SubscriptionType> _dataHelper;

        public SubscriptionsTypesViewModel(IDataHelper<SubscriptionType> dataHelper)
        {
            _dataHelper = dataHelper;
            Task.Run(async () => await LoadData());
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
        private async Task OnGetSubscriptionsTypes()
        {
            Task.Run(async () => await LoadData());
        }

        public async Task LoadData(int Page = 1,int Limit = 50)
        {
            IsLoading = true;

            try
            {
                var res = await _dataHelper.GetAllAsync(Page,Limit);
                SubscriptionsTypes = res;
            }
            catch (Exception ex)
            {
                var mb = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok"
                };
                await mb.ShowDialogAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchData()
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
    }
}
