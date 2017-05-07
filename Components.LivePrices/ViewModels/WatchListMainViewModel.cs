using DeepInsights.Components.WatchList.Models;
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

namespace DeepInsights.Components.WatchList.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class WatchListMainViewModel : BindableBase
    {
        #region Private Fields

        private bool _HasQuotesLoaded;
        private readonly IForexWatchListService _ForexWatchListService;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public WatchListMainViewModel(IForexWatchListService forexWatchListService)
        {
            InitializeCommands();
            Instruments = new RangeObservableCollection<Instrument>();

            if (forexWatchListService == null)
            {
                throw new ArgumentNullException("forexWatchListService");
            }
            _ForexWatchListService = forexWatchListService;
        }

        #endregion

        #region Properties

        public string ModuleHeader
        {
            get { return "Market Prices"; }
        }

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
            await FetchWatchList();
        }

        private async Task FetchWatchList()
        {
            var quoteNames = new List<string>
            {
                CurrencyConstants.AUD_CAD, CurrencyConstants.AUD_CHF, CurrencyConstants.AUD_JPY, CurrencyConstants.AUD_NZD, CurrencyConstants.AUD_USD,
                CurrencyConstants.CAD_CHF, CurrencyConstants.CAD_JPY, CurrencyConstants.CHF_JPY, CurrencyConstants.EUR_AUD, CurrencyConstants.EUR_CAD,
                CurrencyConstants.EUR_CHF, CurrencyConstants.EUR_GBP, CurrencyConstants.EUR_JPY, CurrencyConstants.EUR_NZD
            };
            string forexPricesJson = await _ForexWatchListService.GetLiveForexPricesJson(quoteNames);
            dynamic pricesResult = JsonConvert.DeserializeObject(forexPricesJson);

            var forexInstruments = new List<Instrument>();
            foreach (dynamic price in pricesResult.prices)
            {
                string quoteName = price.instrument;
                decimal bid = price.bids[0].price;
                decimal ask = price.asks[0].price;
                decimal spread = ask - bid;
                decimal lowestBid = bid;
                decimal highestAsk = ask;

                forexInstruments.Add(new ForexInstrument { QuoteName = quoteName, Spread = spread, Bid = bid, Ask = ask, LowestBid = lowestBid, HighestAsk = highestAsk });
            }

            Instruments.ClearAndAddRange(forexInstruments);
            HasQuotesLoaded = true;
        }

        #endregion
    }
}
