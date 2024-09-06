using GMSMAG.ViewModels.Pages;
using System.Windows.Controls;

namespace GMSMAG.Views.UserControls
{
    public partial class ManageSubscriptionType : UserControl
    {
        public ManageSubscriptionType(SubscriptionsTypesViewModel subscriptionsTypesViewModel)
        {
            DataContext = subscriptionsTypesViewModel;
            InitializeComponent();
        }
    }
}
