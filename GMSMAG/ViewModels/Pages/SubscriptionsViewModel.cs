using GMSMAG.Data;
using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.ViewModels.Pages
{
    public partial class SubscriptionsViewModel :ObservableObject
    {
        private readonly IDataHelper<Subscription> _dataHelper;

        public SubscriptionsViewModel(IDataHelper<Subscription> dataHelper)
        {
            _dataHelper = dataHelper;
            Task.Run(async () => await OnGetSubscriptions());
        }

        [ObservableProperty]
        private Subscription subscription;
        [ObservableProperty]
        private Subscription selectedSubscription;
        [ObservableProperty]
        private List<Subscription> subscriptions = new List<Subscription>();
        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private string colName;
        [ObservableProperty]
        private string searchText;


        [RelayCommand]
        private async Task OnGetSubscriptions()
        {
            await Load(1,50);
        }

        public async Task Load(int Page = 1, int Limit = 50)
        {
            IsLoading = true;

            try
            {
                var res = await _dataHelper.GetAllAsync(Page,Limit);
                Subscriptions = res;
            }
            catch (Exception er)
            {

                var messageBox = new Wpf.Ui.Controls.MessageBox
                {
                    Width = 300,
                    Height = 150,
                    Title = "Error",
                    Content = er.Message.ToString(),
                    CloseButtonText = "Ok"
                };

                await messageBox.ShowDialogAsync();
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
