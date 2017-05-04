using Microsoft.Practices.Prism.Mvvm;
using System.ComponentModel.Composition;

namespace DeepInsights.Components.HistoricalPrices.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HistoricalPricesMainViewModel : BindableBase
    {
    }
}
