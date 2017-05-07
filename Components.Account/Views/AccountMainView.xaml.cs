using DeepInsights.Components.Account.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.Account.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AccountMainView : UserControl
    {
        [ImportingConstructor]
        public AccountMainView(AccountMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
