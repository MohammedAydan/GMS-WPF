using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace GMSMAG.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "WPF UI - GMSMAG";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Subscribers",
                Icon = new SymbolIcon { Symbol = SymbolRegular.PeopleCommunity20 },
                TargetPageType = typeof(Views.Pages.SubscribersPage)
            },
            new NavigationViewItem()
            {
                Content = "Subscriptions",
                Icon = new SymbolIcon { Symbol = SymbolRegular.PeopleCheckmark16 },
                TargetPageType = typeof(Views.Pages.SubscriptionsPage)
            },
            new NavigationViewItem()
            {
                Content = "Subscriptions Types",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentRibbon16 },
                TargetPageType = typeof(Views.Pages.SubscriptionsTypesPage)
            },
            //new NavigationViewItem()
            //{
            //    Content = "Data",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            //    TargetPageType = typeof(Views.Pages.DataPage)
            //}
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
