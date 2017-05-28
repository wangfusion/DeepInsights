using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using System.ComponentModel.Composition;
using System.Windows;

namespace DeepInsights.Shell.Infrastructure.RegionBehaviors
{
    [Export(typeof(TabbedGroupRegionBehavior))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TabbedGroupRegionBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
        #region Private Fields

        private readonly IRegionManager _RegionManager;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public TabbedGroupRegionBehavior(IRegionManager regionManager)
        {
            _RegionManager = regionManager;
        }

        #endregion

        #region IHostAwareRegionBehavior overrides

        public DependencyObject HostControl
        {
            get;
            set;
        }

        #endregion

        #region RegionBehavior overrides

        protected override void OnAttach()
        {
            RegisterRegion();
        }

        #endregion

        #region Private Methods

        private void RegisterRegion()
        {
            DependencyObject targetElement = HostControl;
            if (targetElement.CheckAccess())
            {
                TabbedGroup tabbedGroup = targetElement as TabbedGroup;
                if (tabbedGroup != null && _RegionManager != null)
                {
                    _RegionManager.Regions.Add(Region);
                }
            }
        }

        #endregion
    }
}
