using DeepInsights.Components.HistoricalPrices.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.HistoricalPrices.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class HistoricalPricesMainView : UserControl
    {
        [ImportingConstructor]
        public HistoricalPricesMainView(HistoricalPricesMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public string GetPanelCaption()
        {
            return "Chart";
        }
    }
}
