using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Shell.Infrastructure.RegionAdapters
{
    [Export(typeof(DockManagerAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DockManagerAdapter : RegionAdapterBase<DockLayoutManager>
    {
        #region Constructor

        [ImportingConstructor]
        public DockManagerAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory) { }

        #endregion

        #region RegionAdapterBase Overrides

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        protected override void Adapt(IRegion region, DockLayoutManager regionTarget)
        {
            BaseLayoutItem[] items = regionTarget.GetItems();
            foreach (BaseLayoutItem item in items)
            {
                string regionName = RegionManager.GetRegionName(item);
                if (!string.IsNullOrEmpty(regionName))
                {
                    LayoutPanel panel = item as LayoutPanel;
                    if (panel != null && panel.Content == null)
                    {
                        var control = new ContentControl();
                        RegionManager.SetRegionName(control, regionName);
                        panel.Content = control;
                    }
                }
            }
        }

        #endregion
    }
}
