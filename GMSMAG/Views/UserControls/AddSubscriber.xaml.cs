using GMSMAG.Models;
using GMSMAG.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GMSMAG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddSubscriber.xaml
    /// </summary>
    public partial class AddSubscriber : UserControl
    {
        public AddSubscriber(SubscribersViewModel subscribersViewModel,List<SubscriptionType> subscriptionsTypes)
        {
            InitializeComponent();
            DataContext = subscribersViewModel;
            subscriptionsTypesComboBox.Items.Clear();
            subscriptionsTypesComboBox.ItemsSource = subscriptionsTypes;
            subscriptionsTypesComboBox.DisplayMemberPath = "SubscriptionNamePrice";
            subscriptionsTypesComboBox.SelectedValuePath = "Id";
            subscriptionsTypesComboBox.SelectedIndex = 0;
        }
    }
}
