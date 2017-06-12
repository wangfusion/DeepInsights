using DeepInsights.Components.Account.Views;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Utilities;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;

namespace DeepInsights.Components.Account
{
    [ModuleExport(typeof(AccountModule), InitializationMode=InitializationMode.WhenAvailable)]
    public class AccountModule : IModule
    {
        #region Private Fields

        private readonly IRegionManager _RegionManager;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AccountModule(IRegionManager regionManager)
        {
            regionManager.ThrowIfNull("regionManager");

            _RegionManager = regionManager;
        }

        #endregion

        #region IModule Implementation

        public void Initialize()
        {
            _RegionManager.RegisterViewWithRegion(RegionNames.RightTopRegion, typeof(AccountMainView));
        }

        #endregion
    }
}
