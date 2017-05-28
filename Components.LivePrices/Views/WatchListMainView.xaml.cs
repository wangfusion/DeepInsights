using DeepInsights.Components.WatchList.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.WatchList.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class WatchListMainView : UserControl
    {
        [ImportingConstructor]
        public WatchListMainView(WatchListMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public string GetPanelCaption()
        {
            return "Watch List";
        }
    }
}
