using DevExpress.Xpf.Core;
using Microsoft.Practices.Prism.Mvvm;
using System.ComponentModel.Composition;

namespace DeepInsights.Shell
{
    [Export]
    public partial class Shell : DXWindow, IView
    {
        public Shell()
        {
            InitializeComponent();
            Title = "DeepInsights Beta";
        }
    }
}