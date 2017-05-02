using DeepInsights.Shell.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using System.Net;

namespace DeepInsights.Components.LivePrices.ViewModels
{
    public class LivePricesMainViewModel : BindableBase
    {
        #region Private Fields

        private string _TestPrice;

        #endregion

        #region Constructor

        public LivePricesMainViewModel()
        {
            InitializeCommands();         
        }

        #endregion

        #region Properties

        public string TestPrice
        {
            get { return _TestPrice; }
            set { SetProperty(ref _TestPrice, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand RefreshPricesCommand
        {
            get;
            private set;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            RefreshPricesCommand = new DelegateCommand(FetchLivePrices);
        }

        private void FetchLivePrices()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", "Bearer " + ApplicationConstants.FX_TOKEN);
                webClient.QueryString.Add("instruments", CurrencyConstants.EUR_USD);

                string result = webClient.DownloadString(ApplicationConstants.FX_URL + ApplicationConstants.FX_PRICING_ENDPOINT);
                dynamic prices = JsonConvert.DeserializeObject(result);
                dynamic price = prices.prices[0];

                double bid = price.bids[0].price;
                double ask = price.asks[0].price;
            }
        }

        #endregion
    }
}
