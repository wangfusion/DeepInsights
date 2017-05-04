using DeepInsights.Components.MarketNews.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.MarketNews.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MarketNewsMainView : UserControl
    {
        [ImportingConstructor]
        public MarketNewsMainView(MarketNewsMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
