using GMSMAG.ViewModels.Pages;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    /// <summary>
    /// Interaction logic for SubscriptionsTypesPage.xaml
    /// </summary>
    public partial class SubscriptionsTypesPage : INavigableView<SubscriptionsTypesViewModel>
    {
        public SubscriptionsTypesViewModel ViewModel { get; }

        public SubscriptionsTypesPage(SubscriptionsTypesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
            InitSearchComboBox();
        }

        private void InitSearchComboBox()
        {
            searchComboBox.Items.Clear();
            searchComboBox.Items.Add(new { Id = "Id", Name = "ID" });
            searchComboBox.Items.Add(new { Id = "SubscriptionName", Name = "Subscription Name" });
            searchComboBox.Items.Add(new { Id = "Price", Name = "Price" });
            searchComboBox.DisplayMemberPath = "Name";
            searchComboBox.SelectedValuePath = "Id";
            searchComboBox.SelectedIndex = 0;
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () => await ViewModel.LoadDataCommand.ExecuteAsync(1));
        }
    }
}
