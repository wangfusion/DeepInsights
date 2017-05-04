using DeepInsights.Components.LivePrices.Models;
using DeepInsights.Services;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Utility;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace DeepInsights.Components.LivePrices.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LivePricesMainViewModel : BindableBase
    {
        #region Private Fields

        private bool _HasQuotesLoaded;
        private readonly IForexLivePricesService _ForexLivePricesService;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public LivePricesMainViewModel(IForexLivePricesService forexLivePricesService)
        {
            InitializeCommands();
            Instruments = new RangeObservableCollection<Instrument>();

            if (forexLivePricesService == null)
            {
                throw new ArgumentNullException("forexLivePricesService");
            }
            _ForexLivePricesService = forexLivePricesService;
        }

        #endregion

        #region Properties

        public RangeObservableCollection<Instrument> Instruments
        {
            get;
            set;
        }

        public bool HasQuotesLoaded
        {
            get { return _HasQuotesLoaded; }
            set
            {
                if (_HasQuotesLoaded != value)
                {
                    SetProperty(ref _HasQuotesLoaded, value);
                    OnPropertyChanged(() => HasQuotesLoaded);
                }
            }
        }

        #endregion

        #region Commands

        public DelegateCommand ViewLoadedCommand
        {
            get;
            private set;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            ViewLoadedCommand = new DelegateCommand(ViewLoaded);
        }

        private async void ViewLoaded()
        {
            await FetchLivePrices();
        }

        private async Task FetchLivePrices()
        {
            var quoteNames = new List<string>
            {
                CurrencyConstants.AUD_CAD, CurrencyConstants.AUD_CHF, CurrencyConstants.AUD_JPY, CurrencyConstants.AUD_NZD, CurrencyConstants.AUD_USD,
                CurrencyConstants.CAD_CHF, CurrencyConstants.CAD_JPY, CurrencyConstants.CHF_JPY, CurrencyConstants.EUR_AUD, CurrencyConstants.EUR_CAD,
                CurrencyConstants.EUR_CHF, CurrencyConstants.EUR_GBP, CurrencyConstants.EUR_JPY, CurrencyConstants.EUR_NZD
            };
            string forexPricesJson = await _ForexLivePricesService.GetLiveForexPricesJson(quoteNames);
            dynamic pricesResult = JsonConvert.DeserializeObject(forexPricesJson);

            var forexInstruments = new List<Instrument>();
            foreach (dynamic price in pricesResult.prices)
            {
                string quoteName = price.instrument;
                double bid = price.bids[0].price;
                double ask = price.asks[0].price;
                double spread = ask - bid;
                double lowestBid = bid;
                double highestAsk = ask;

                forexInstruments.Add(new ForexInstrument { QuoteName = quoteName, Spread = spread, Bid = bid, Ask = ask, LowestBid = lowestBid, HighestAsk = highestAsk });
            }

            Instruments.ClearAndAddRange(forexInstruments);
            HasQuotesLoaded = true;
        }

        #endregion
    }
}
