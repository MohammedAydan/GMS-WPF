using GMSMAG.ViewModels.UserControls;
using GMSMAG.ViewModels.Windows;
using GMSMAG.Views.UserControls;
using GMSMAG.Views.Windows;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class SubscribersPage : INavigableView<SubscribersViewModel>
    {
        public SubscribersViewModel ViewModel { get; }

        public SubscribersPage(SubscribersViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();

            initSItemForSearchComboBox();
        }

        private void initSItemForSearchComboBox()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => ViewModel.LoadData());
        }
    }
}
