﻿using DeepInsights.Components.LivePrices.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DeepInsights.Components.LivePrices.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LivePricesMainView : UserControl
    {
        [ImportingConstructor]
        public LivePricesMainView(LivePricesMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
