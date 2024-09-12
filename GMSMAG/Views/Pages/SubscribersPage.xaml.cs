using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using GMSMAG.ViewModels.Pages;
using GMSMAG.ViewModels.UserControls;
using GMSMAG.Views.Windows;
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class SubscribersPage : INavigableView<SubscribersViewModel>
    {
        public SubscribersViewModel ViewModel { get; }

        public SubscribersPage(SubscribersViewModel viewModel)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = viewModel;
            InitializeComponent();
            InitSearchComboBox();
        }

        private void InitSearchComboBox()
        {
            var searchOptions = new List<dynamic>
            {
                new { Id = "Id", Name = "Id" },
                new { Id = "FirstName", Name = "First Name" },
                new { Id = "LastName", Name = "Last Name" },
                new { Id = "PhoneNumber", Name = "Phone Number" },
                new { Id = "HomePhoneNumber", Name = "Home Phone Number" }
            };

            searchComboBox.ItemsSource = searchOptions;
            searchComboBox.DisplayMemberPath = "Name";
            searchComboBox.SelectedValuePath = "Id";
            searchComboBox.SelectedIndex = 0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (ViewModel == null)
            {
                await ShowErrorAsync("Error", "ViewModel is not initialized.");
                return;
            }

            try
            {
                await ViewModel.LoadData();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync("Error", ex.Message);
            }
        }

        private async Task ShowErrorAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
                DialogHost = ((MainWindow)Application.Current.MainWindow).RootContentDialog
            };
            await dialog.ShowAsync();
        }
    }
}
