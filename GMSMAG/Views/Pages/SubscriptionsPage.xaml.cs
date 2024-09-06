using GMSMAG.ViewModels.Pages;
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
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class SubscriptionsPage : INavigableView<SubscriptionsViewModel>
    {
        public SubscriptionsViewModel ViewModel { get; }

        public SubscriptionsPage(SubscriptionsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            InitSearchComboBox();
        }

        private void InitSearchComboBox()
        {
            searchComboBox.Items.Clear();
            searchComboBox.Items.Add(new { Id = "Id", Name = "Id" });
            searchComboBox.Items.Add(new { Id = "SubscriberId", Name = "Subscriber Id" });
            searchComboBox.Items.Add(new { Id = "SubscriptionTypeId", Name = "Subscription Type Id" });
            searchComboBox.DisplayMemberPath = "Name";
            searchComboBox.SelectedValuePath = "Id";
            searchComboBox.SelectedIndex = 0;
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>await ViewModel.Load(1,50));
        }
    }
}
