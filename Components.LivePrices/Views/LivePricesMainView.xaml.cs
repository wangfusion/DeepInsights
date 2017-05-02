using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.LivePrices.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LivePricesMainView : UserControl
    {
        public LivePricesMainView()
        {
            InitializeComponent();
        }
    }
}
