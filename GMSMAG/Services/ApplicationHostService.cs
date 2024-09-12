using GMSMAG.Views.Pages;
using GMSMAG.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui;

namespace GMSMAG.Services
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private INavigationWindow _navigationWindow;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync(cancellationToken);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Add any necessary cleanup code here
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync(CancellationToken cancellationToken)
        {
            // Ensure no other InitAppWindow instances are open
            if (!Application.Current.Windows.OfType<InitAppWindow>().Any())
            {
                _navigationWindow = (_serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow)!;
                if (_navigationWindow != null)
                {
                    _navigationWindow.ShowWindow();

                    // Navigate to the initial page
                    //_navigationWindow.Navigate(typeof(DashboardPage));
                }
                else
                {
                    // Log or handle the case where INavigationWindow is not resolved
                    // e.g., throw new InvalidOperationException("INavigationWindow service is not registered.");
                }
            }

            await Task.CompletedTask;
        }
    }
}
