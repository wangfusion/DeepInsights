using System.Windows;
using DevExpress.Xpf.Core;
using System.Windows.Threading;
using System;
using System.Threading.Tasks;

namespace DeepInsights.Shell
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterGlobalExceptionHandling();
            ApplicationThemeHelper.ApplicationThemeName = Theme.MetropolisDarkName;
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void RegisterGlobalExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => OnCurrentDomainUnhandledException(sender, args);
            Dispatcher.UnhandledException += (sender, args) => OnDispatcherUnhandledException(sender, args);
            Current.DispatcherUnhandledException += (sender, args) => OnCurrentDispatcherUnhandledException(sender, args);
            TaskScheduler.UnobservedTaskException += (sender, args) => OnTaskSchedulerUnobservedTaskException(sender, args);
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception;
            var terminatingMessage = args.IsTerminating ? " The application is terminating." : string.Empty;
            var exceptionMessage = exception?.Message ?? "An unmanaged exception occured.";
            var message = string.Concat(exceptionMessage, terminatingMessage);
            // Todo: Add logging
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            // Todo: Log error
        }

        private void OnCurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            #if DEBUG // Let Visual Studio handle the exception in debug mode
                e.Handled = false;
            #else
                ShowUnhandledException(e);
            #endif
        }

        private void ShowUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            // Todo: Add logging here
            string errorMessage = string.Format("An application error occurred.\nPlease check whether your data is correct and repeat the action. If this error occurs again there seems to be a more serious malfunction in the application, and you better close it.\n\nError: {0}\n\nDo you want to continue?\n(if you click Yes you will continue with your work, if you click No the application will close)",
                    e.Exception.Message + (e.Exception.InnerException != null ? "\n" +
                    e.Exception.InnerException.Message : null));

            if (MessageBox.Show(errorMessage, "Application Error", MessageBoxButton.YesNoCancel, MessageBoxImage.Error) == MessageBoxResult.No)
            {
                if (MessageBox.Show("WARNING: The application will close. Any changes will not be saved!\nDo you really want to close it?", "Close the application!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Current.Shutdown();
                }
            }
        }

        private void OnTaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            // Todo: Add logging
            args.SetObserved();
        }
    }
}
