using DeepInsights.Components.LivePrices.Views;
using DeepInsights.Shell.Infrastructure;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;

namespace Components.LivePrices
{
    [ModuleExport(typeof(LivePricesModule), InitializationMode=InitializationMode.WhenAvailable)]
    public class LivePricesModule : IModule
    {
        #region Private Fields

        private readonly IRegionManager _RegionManager;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public LivePricesModule(IRegionManager regionManager)
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
            _RegionManager.RegisterViewWithRegion(RegionNames.LeftMostRegion, typeof(LivePricesMainView));
        }

        #endregion
    }
}
