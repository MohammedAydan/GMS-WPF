using GMSMAG.Models;
using GMSMAG.ViewModels.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace GMSMAG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddSubscriber.xaml
    /// </summary>
    public partial class AddSubscriber : UserControl
    {
        public AddSubscriber(SubscribersViewModel subscribersViewModel, List<SubscriptionType> subscriptionTypes)
        {
            InitializeComponent();

            // Set the DataContext for binding to the ViewModel
            DataContext = subscribersViewModel;

            // Initialize the ComboBox with subscription types
            ConfigureSubscriptionComboBox(subscriptionTypes, subscribersViewModel);
        }

        private void ConfigureSubscriptionComboBox(List<SubscriptionType> subscriptionTypes, SubscribersViewModel subscribersViewModel)
        {
            if (subscriptionTypes == null || !subscriptionTypes.Any()) return;

            // Set the ComboBox ItemsSource and properties
            subscriptionsTypesComboBox.ItemsSource = subscriptionTypes;
            subscriptionsTypesComboBox.DisplayMemberPath = "SubscriptionNamePrice";
            subscriptionsTypesComboBox.SelectedValuePath = "Id";

            // Set initial selection based on the subscriber's first subscription (if available)
            if (subscribersViewModel?.SelectedSubscriber?.Subscriptions?.Any() == true)
            {
                subscriptionsTypesComboBox.SelectedValue = subscribersViewModel.SelectedSubscriber.Subscriptions.First();
            }
        }
    }
}
