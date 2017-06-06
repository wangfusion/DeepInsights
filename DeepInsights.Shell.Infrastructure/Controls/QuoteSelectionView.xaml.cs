using DevExpress.Xpf.Core;
using System.Windows.Controls;

namespace DeepInsights.Shell.Infrastructure.Controls
{
    public partial class QuoteSelectionView : UserControl
    {
        public QuoteSelectionView()
        {
            InitializeComponent();
            DataContext = new QuoteSelectionViewModel();
            ThemeManager.SetThemeName(this, Theme.MetropolisDarkName);
        }
    }
}
