using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using GMSMAG.Data;
using GMSMAG.Models;
using GMSMAG.ViewModels.Pages;
using GMSMAG.ViewModels.UserControls;
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class SubscribersPage : INavigableView<SubscribersViewModel>
    {
        public SubscribersViewModel ViewModel { get; }
        private PrintViewModel _printViewModel;

        public SubscribersPage(SubscribersViewModel viewModel)
        {
            _printViewModel = new PrintViewModel();
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();

            InitSearchComboBox();
        }

        private void InitSearchComboBox()
        {
            searchComboBox.Items.Clear();
            searchComboBox.Items.Add(new { Id = "Id", Name = "Id" });
            searchComboBox.Items.Add(new { Id = "FirstName", Name = "First name" });
            searchComboBox.Items.Add(new { Id = "LastName", Name = "Last name" });
            searchComboBox.Items.Add(new { Id = "PhoneNumber", Name = "Phone number" });
            searchComboBox.Items.Add(new { Id = "HomePhoneNumber", Name = "H-Phone number" });
            searchComboBox.DisplayMemberPath = "Name";
            searchComboBox.SelectedValuePath = "Id";
            searchComboBox.SelectedIndex = 0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => ViewModel.LoadData());
        }

        private async void PrintDataGrid(object sender, RoutedEventArgs e)
        {
            await Task.Run(async() => await _printViewModel.GenerateAndSavePdfAsync(dataGrid,"D:\\Downloads\\GMS-REPORT-PDF.pdf"));
        }

    }
}
