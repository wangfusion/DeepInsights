using DeepInsights.Components.Account;
using DeepInsights.Components.HistoricalPrices;
using DeepInsights.Components.WatchList;
using DeepInsights.Components.MarketNews;
using DeepInsights.Services;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using ModuleCatalogInstance = Microsoft.Practices.Prism.Modularity.ModuleCatalog;

namespace DeepInsights.Shell
{
    public class Bootstrapper : MefBootstrapper
    {
        #region MefBootstrapper Overrides

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return ModuleCatalogInstance.CreateFromXaml(new Uri("/DeepInsights.Shell;component/DeepInsights.Shell.ModulesCatalog.xaml", UriKind.Relative));
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MarketNewsModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(WatchListModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(HistoricalPricesModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(IForexWatchListService).Assembly));
        }

        #endregion
    }
}
