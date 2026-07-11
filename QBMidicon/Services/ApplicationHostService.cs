using Microsoft.Extensions.Hosting;
using QBMidicon.Class.Services;
using QBMidicon.Contracts.Activation;
using QBMidicon.Contracts.Services;
using QBMidicon.Contracts.Views;
using QBMidicon.ViewModels;

namespace QBMidicon.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IEnumerable<IActivationHandler> _activationHandlers;
        private IShellWindow _shellWindow;
        private bool _isInitialized;

        private readonly ISettingService _settingService;

        private readonly IPageService _pageService;

        public ApplicationHostService(
            IServiceProvider serviceProvider,
            IEnumerable<IActivationHandler> activationHandlers,
            INavigationService navigationService,
            IPageService pageService,
            ISettingService settingService)
        {
            _serviceProvider = serviceProvider;
            _activationHandlers = activationHandlers;
            _navigationService = navigationService;
            _pageService = pageService;

            _settingService = settingService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            await HandleActivationAsync();

            // Tasks after activation
            await StartupAsync();
            _isInitialized = true;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        private async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                await Task.CompletedTask;
            }
        }

        private async Task StartupAsync()
        {
            if (!_isInitialized)
            {
                await Task.CompletedTask;
            }
        }

        private async Task HandleActivationAsync()
        {
            var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle());

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync();
            }

            await Task.CompletedTask;

            if (System.Windows.Application.Current.Windows.OfType<IShellWindow>().Count() == 0)
            {
                // Default activation that navigates to the apps default page
                _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
                _navigationService.Initialize(_shellWindow.GetNavigationFrame());
                _shellWindow.ShowWindow();
                _navigationService.NavigateTo(typeof(SourceDataViewModel).FullName);
                await Task.CompletedTask;
            }
        }
    }
}
