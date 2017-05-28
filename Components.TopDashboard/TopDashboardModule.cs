using DeepInsights.Components.TopDashboard.Views;
using DeepInsights.Shell.Infrastructure;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;

namespace DeepInsights.Components.TopDashboard
{
    [ModuleExport(typeof(TopDashboardModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class TopDashboardModule : IModule
    {
        #region Private Fields

        private readonly IRegionManager _RegionManager;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public TopDashboardModule(IRegionManager regionManager)
        {
            if (regionManager == null) throw new ArgumentNullException("regionManager");

            _RegionManager = regionManager;
        }

        #endregion

        #region IModule Implementation

        public void Initialize()
        {
            _RegionManager.RegisterViewWithRegion(RegionNames.TopDashboardRegion, typeof(TopDashboardMainView));
        }

        #endregion
    }
}
