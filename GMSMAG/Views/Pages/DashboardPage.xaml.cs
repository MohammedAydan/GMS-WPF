using GMSMAG.ViewModels.Pages;
using Wpf.Ui.Controls;
using GMSMAG.Views.Windows;
using GMSMAG.ViewModels.UserControls;
using System.Windows.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
