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
    /// Interaction logic for HeaderReport.xaml
    /// </summary>
    public partial class HeaderReport : UserControl
    {
        public HeaderReport()
        {
            InitializeComponent();
            dateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
        }
    }
}
