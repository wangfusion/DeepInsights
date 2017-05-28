using DeepInsights.Components.TopDashboard.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.TopDashboard.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TopDashboardMainView : UserControl
    {
        [ImportingConstructor]
        public TopDashboardMainView(TopDashboardMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public string GetPanelCaption()
        {
            return "";
        }
    }
}
