using GMSMAG.ViewModels.Pages;
using GMSMAG.ViewModels.UserControls;
using GMSMAG.Views.Pages;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace GMSMAG.Views.Windows
{
    /// <summary>
    /// Interaction logic for InitAppWindow.xaml
    /// </summary>
    public partial class InitAppWindow : INavigationWindow
    {
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly SubscribersViewModel _subscribersViewModel;
        private readonly SubscriptionsTypesViewModel _subscriptionsTypesViewModel;
        private readonly SubscriptionsViewModel _subscriptionsViewModel;
        private double _progressValue = 0.0;

        public InitAppWindow(DashboardViewModel dashboardViewModel, SubscribersViewModel subscribersViewModel, SubscriptionsTypesViewModel subscriptionsTypesViewModel, SubscriptionsViewModel subscriptionsViewModel)
        {
            _dashboardViewModel = dashboardViewModel ?? throw new ArgumentNullException(nameof(dashboardViewModel));
            _subscribersViewModel = subscribersViewModel ?? throw new ArgumentNullException(nameof(subscribersViewModel));
            _subscriptionsTypesViewModel = subscriptionsTypesViewModel ?? throw new ArgumentNullException(nameof(subscriptionsTypesViewModel));
            _subscriptionsViewModel = subscriptionsViewModel ?? throw new ArgumentNullException(nameof(subscriptionsViewModel));

            InitializeComponent();
            SystemThemeWatcher.Watch(this);

            Loaded += InitAppWindow_Loaded;
        }

        private async void InitAppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or notify the user
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {
                    Width = 300,
                    Height = 150,
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok"
                };
                await messageBox.ShowDialogAsync();
            }
            finally
            {
                //Hide();
                //var mainWindow = App.GetService<MainWindow>();
                //mainWindow?.ShowWindow();
                //mainWindow?.Navigate(typeof(DashboardPage));
            }
        }

        private async Task LoadDataAsync()
        {
            // Assuming you have a progress bar named 'ProgressBar' in your XAML
            // Update the progress value before starting data loading
            _progressValue = 0.0;
            UpdateProgressBar();
            await Task.Delay(200);

            // Load dashboard data
            await _dashboardViewModel.Load();
            _progressValue = 25.0; // Update progress after loading dashboard data
            UpdateProgressBar();
            await Task.Delay(200);

            // Load subscribers data
            await _subscribersViewModel.LoadData();
            _progressValue = 50.0; // Update progress after loading subscribers data
            UpdateProgressBar();
            await Task.Delay(200);

            // Load subscription types data
            await _subscriptionsTypesViewModel.LoadData();
            _progressValue = 75.0; // Update progress after loading subscription types data
            UpdateProgressBar();
            await Task.Delay(200);

            // Load subscriptions data
            await _subscriptionsViewModel.LoadSubscriptionsCommand.ExecuteAsync(1);
            _progressValue = 100.0; // Update progress after loading subscriptions data
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            // Assuming you have a ProgressBar control named 'ProgressBar' in your XAML
            if (progressBar != null)
            {
                if(progressBar.IsIndeterminate == true)
                {
                    progressBar.IsIndeterminate = false;
                }

                progressBar.Value = _progressValue;
                //initializingText.Text = "Initializing... " + _progressValue + "%";
            }
        }

        public void CloseWindow() => Close();

        public INavigationView GetNavigation()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow() => Show();
    }
}
