using GMSMAG.ViewModels.Pages;
using System.Windows.Controls;

namespace GMSMAG.Views.UserControls
{
    public partial class ManageSubscriptions : UserControl
    {
        public ManageSubscriptions(SubscriptionsViewModel subscriptionsViewModel)
        {
            InitializeComponent();
            DataContext = subscriptionsViewModel;
        }
    }
}
