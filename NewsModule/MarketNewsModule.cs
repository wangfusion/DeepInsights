using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Regions;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Components.MarketNews.Views;

namespace DeepInsights.Components.MarketNews
{
    [ModuleExport(typeof(MarketNewsModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class MarketNewsModule : IModule
    {
        #region Private Fields

        private readonly IRegionManager _RegionManager;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public MarketNewsModule(IRegionManager regionManager)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            _RegionManager = regionManager;
        }

        #endregion

        #region IModule Implementation

        public void Initialize()
        {
            _RegionManager.RegisterViewWithRegion(RegionNames.RightMostRegion, typeof(MarketNewsMainView));
        }

        #endregion
    }
}
