using System;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Modularity;
using ModuleCatalogInstance = Microsoft.Practices.Prism.Modularity.ModuleCatalog;
using DeepInsights.Components.MarketNews;
using System.ComponentModel.Composition.Hosting;
using Components.LivePrices;

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
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(LivePricesModule).Assembly));
        }

        #endregion
    }
}
