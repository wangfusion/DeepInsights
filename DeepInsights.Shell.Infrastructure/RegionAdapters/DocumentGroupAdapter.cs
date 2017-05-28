using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System;

namespace DeepInsights.Shell.Infrastructure.RegionAdapters
{
    [Export(typeof(DocumentGroupAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DocumentGroupAdapter : RegionAdapterBase<DocumentGroup>
    {
        #region Constructor

        [ImportingConstructor]
        public DocumentGroupAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory) { }

        #endregion

        #region RegionAdapterBase overrides

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, DocumentGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                OnViewsCollectionChanged(region, regionTarget, s, e);
            };
        }

        #endregion

        #region Private Methods

        private void OnViewsCollectionChanged(IRegion region, DocumentGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object view in e.NewItems)
                {
                    DockLayoutManager manager = regionTarget.GetDockLayoutManager();
                    DocumentPanel panel = manager.DockController.AddDocumentPanel(regionTarget);
                    panel.Content = view;
                    if (view is IPanelInfo)
                    {
                        panel.Caption = ((IPanelInfo)view).GetPanelCaption();
                    }
                    else
                    {
                        panel.Caption = "new Page";
                    }
                    manager.DockController.Activate(panel);
                }
            }
        }

        #endregion
    }
}
