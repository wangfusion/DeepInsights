using System.Collections.Specialized;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using DevExpress.Xpf.Docking;

namespace DeepInsights.Shell.Infrastructure.RegionAdapters
{
    [Export(typeof(LayoutGroupAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LayoutGroupAdapter : RegionAdapterBase<LayoutGroup>
    {
        #region Private Fields

        private bool _LockItemsChanged;
        private bool _LockViewsChanged;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public LayoutGroupAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory) { }

        #endregion

        #region RegionAdapterBase overrides

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, LayoutGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
            regionTarget.Items.CollectionChanged += (s, e) => OnItemsCollectionChanged(region, regionTarget, s, e);
        }

        #endregion

        #region Private Methods

        private void OnItemsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_LockItemsChanged)
            {
                return;
            }
        }

        private void OnViewsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_LockViewsChanged)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var panel = new LayoutPanel { Content = view };
                    if (view is IPanelInfo)
                    {
                        panel.Caption = ((IPanelInfo)view).GetPanelCaption();
                    }
                    else
                    {
                        panel.Caption = "new Page";
                    }

                    _LockItemsChanged = true;
                    regionTarget.Items.Add(panel);
                    _LockItemsChanged = false;

                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var view in e.OldItems)
                {
                    LayoutPanel viewPanel = null;
                    foreach (LayoutPanel panel in regionTarget.Items)
                    {
                        if (panel.Content == view)
                        {
                            viewPanel = panel;
                            break;
                        }
                    }

                    if (viewPanel == null) continue;
                    viewPanel.Content = null;
                    _LockItemsChanged = true;
                    regionTarget.Items.Remove(viewPanel);
                    _LockItemsChanged = false;
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }

        #endregion
    }
}
