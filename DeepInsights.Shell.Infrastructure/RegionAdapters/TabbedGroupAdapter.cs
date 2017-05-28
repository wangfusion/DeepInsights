using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System;

namespace DeepInsights.Shell.Infrastructure.RegionAdapters
{
    [Export(typeof(TabbedGroupAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TabbedGroupAdapter : RegionAdapterBase<TabbedGroup>
    {
        #region Constructor

        public TabbedGroupAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory) { }

        #endregion

        #region RegionAdapterBase overrides

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, TabbedGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                OnViewsCollectionChanged(region, regionTarget, s, e);
            };
        }

        #endregion

        #region Private Methods

        private void OnViewsCollectionChanged(IRegion region, TabbedGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object view in e.NewItems)
                {
                    var panel = new LayoutPanel();
                    panel.Content = view;
                    panel.Caption = "new Page";
                    regionTarget.Items.Add(panel);
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }

        #endregion
    }
}
