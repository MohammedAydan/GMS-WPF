using GMSMAG.ViewModels.UserControls;
using System.Windows.Controls;

namespace GMSMAG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SubscribersView.xaml
    /// </summary>
    public partial class SubscribersView : UserControl
    {
        public SubscribersView()
        {
            InitializeComponent();
            this.DataContext = App.GetService<SubscribersViewModel>();
            initSItemForSearchComboBoc();
        }

        private void initSItemForSearchComboBoc()
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
    }
}
