using GMSMAG.ViewModels.Windows;
using GMSMAG.Views.UserControls;
using GMSMAG.Views.Windows;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui;
using ui = Wpf.Ui.Controls;

namespace GMSMAG.Views.Pages
{
    public partial class SubscribersPage : Page
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

        public SubscribersPage()
        {
            InitializeComponent();

            subscribersStackPanel.Children.Add(new SubscribersView());
        }
    }
}
