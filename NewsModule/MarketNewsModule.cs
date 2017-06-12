using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Regions;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Components.MarketNews.Views;
using DeepInsights.Shell.Infrastructure.Utilities;

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
            regionManager.ThrowIfNull("regionManager");

            _RegionManager = regionManager;
        }

        #endregion

        #region IModule Implementation

        public void Initialize()
        {
            _RegionManager.RegisterViewWithRegion(RegionNames.RightBottomRegion, typeof(MarketNewsMainView));
        }

        #endregion
    }
}
