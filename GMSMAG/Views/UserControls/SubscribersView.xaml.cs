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
    /// Interaction logic for SubscribersView.xaml
    /// </summary>
    public partial class SubscribersView : UserControl
    {
        private SubscribersViewModel subscribersViewModel;

        public SubscribersView()
        {
            InitializeComponent();
            subscribersViewModel = new SubscribersViewModel();
            this.DataContext = subscribersViewModel;
        }
    }
}
