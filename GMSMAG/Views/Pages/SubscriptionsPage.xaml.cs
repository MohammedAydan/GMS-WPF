using GMSMAG.ViewModels.Pages;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class SubscriptionsPage : INavigableView<SubscriptionsViewModel>
    {
        public SubscriptionsViewModel ViewModel { get; }

        public SubscriptionsPage(SubscriptionsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
            InitSearchComboBox();
        }

        private void InitSearchComboBox()
        {
            var searchOptions = new ObservableCollection<object>
            {
                new { Id = "Id", Name = "Id" },
                new { Id = "SubscriberId", Name = "Subscriber Id" },
                new { Id = "SubscriptionTypeId", Name = "Subscription Type Id" }
            };

            searchComboBox.Items.Clear();
            searchComboBox.ItemsSource = searchOptions;
            searchComboBox.DisplayMemberPath = "Name";
            searchComboBox.SelectedValuePath = "Id";
            searchComboBox.SelectedIndex = 0;
        }

        private async void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.LoadSubscriptionsCommand.ExecuteAsync(1);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error reloading data: {ex.Message}");
            }
        }
    }
}
