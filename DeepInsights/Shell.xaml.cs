using Microsoft.Practices.Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;

namespace DeepInsights.Shell
{
    [Export]
    public partial class Shell : Window, IView
    {
        public Shell()
        {
            InitializeComponent();
        }
    }
}
