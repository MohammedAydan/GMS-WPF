using GMSMAG.Core;
using GMSMAG.Data.SqlOpreations;
using GMSMAG.ViewModels.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSMAG.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IDashboardEntity _dashboardEntity;

        public DashboardViewModel(IDashboardEntity dashboardEntity)
        {
            _dashboardEntity = dashboardEntity;
            Task.Run(() => Load());
        }

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private List<SubscriberInfo> expiredSubscribers = new List<SubscriberInfo>();

        [ObservableProperty]
        private int subscribersCount;

        [ObservableProperty]
        private int subscriptionsCount;

        [ObservableProperty]
        private int subscriptionsTypesCount;

        [RelayCommand]
        private async Task OnGetSubscribersCount()
        {
            SubscribersCount = await _dashboardEntity.GetSubscribersCountAsync();
        }

        [RelayCommand]
        private async Task OnGetSubscriptionsCount()
        {
            SubscriptionsCount = await _dashboardEntity.GetSubscriptionsCountAsync();
        }

        [RelayCommand]
        private async Task OnGetSubscriptionsTypesCount()
        {
            SubscriptionsTypesCount = await _dashboardEntity.GetSubscriptionsTypesCountAsync();
        }

        [RelayCommand]
        private async Task OnGetExpiredSubscribers()
        {
            var res = await _dashboardEntity.GetExpiredSubscribersAsync();
            ExpiredSubscribers = res;
        }

        private async void Load()
        {
            IsLoading = true;

            try
            {
                await Task.Run(async () =>
                {
                    await OnGetSubscribersCount();
                    await OnGetSubscriptionsCount();
                    await OnGetSubscriptionsTypesCount();
                    await OnGetExpiredSubscribers();
                });
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
    }
}
