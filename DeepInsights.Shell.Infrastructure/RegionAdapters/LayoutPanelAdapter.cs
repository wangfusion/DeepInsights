using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

namespace DeepInsights.Shell.Infrastructure.RegionAdapters
{
    [Export(typeof(LayoutPanelAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LayoutPanelAdapter : RegionAdapterBase<LayoutPanel>
    {
        #region Constructor

        [ImportingConstructor]
        public LayoutPanelAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory) { }

        #endregion

        #region RegionAdapterBase Overrides

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        protected override void Adapt(IRegion region, LayoutPanel regionTarget)
        {
            region.Views.CollectionChanged += (d, e) =>
            {
                if (e.NewItems != null)
                {
                    regionTarget.Content = e.NewItems[0];
                }
            };
        }

        #endregion
    }
}
