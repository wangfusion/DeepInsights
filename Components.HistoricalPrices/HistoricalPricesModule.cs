using DeepInsights.Components.HistoricalPrices.Views;
using DeepInsights.Shell.Infrastructure;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;

namespace DeepInsights.Components.HistoricalPrices
{
    [ModuleExport(typeof(HistoricalPricesModule), InitializationMode=InitializationMode.WhenAvailable)]
    public class HistoricalPricesModule : IModule
    {
        #region Private Fields

        private readonly IRegionManager _RegionManager;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public HistoricalPricesModule(IRegionManager regionManager)
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
            _RegionManager.RegisterViewWithRegion(RegionNames.CenterRegion, typeof(HistoricalPricesMainView));
        }

        #endregion
    }
}
