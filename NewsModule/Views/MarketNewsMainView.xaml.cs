using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.MarketNews.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MarketNewsMainView : UserControl
    {
        public MarketNewsMainView()
        {
            InitializeComponent();
        }
    }
}
