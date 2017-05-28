using DeepInsights.Components.Account.ViewModels;
using DeepInsights.Shell.Infrastructure;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.Account.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AccountMainView : UserControl, IPanelInfo
    {
        [ImportingConstructor]
        public AccountMainView(AccountMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public string GetPanelCaption()
        {
            return "Account";
        }
    }
}
