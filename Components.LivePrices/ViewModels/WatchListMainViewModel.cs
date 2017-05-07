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
                CurrencyConstants.AUD_CAD, CurrencyConstants.AUD_CHF, CurrencyConstants.AUD_NZD, CurrencyConstants.AUD_USD,
                CurrencyConstants.CAD_CHF, CurrencyConstants.EUR_AUD, CurrencyConstants.EUR_CAD,
                CurrencyConstants.EUR_CHF, CurrencyConstants.EUR_GBP, CurrencyConstants.EUR_NZD
            };
            string forexPricesJson = await _ForexWatchListService.GetLiveForexPricesJson(quoteNames);
            dynamic pricesResult = JsonConvert.DeserializeObject(forexPricesJson);

            var forexInstruments = new List<Instrument>();
            foreach (dynamic price in pricesResult.prices)
            {
                string quoteName = price.instrument;
                string bid = price.bids[0].price;
                string bidFirstPart = bid.Substring(0, 4);
                string bidSecondPart = bid.Substring(4, 2);
                string fractionalBidPip = bid.Substring(6, 1);
                string ask = price.asks[0].price;
                string askFirstPart = ask.Substring(0, 4);
                string askSecondPart = ask.Substring(4, 2);
                string fractionalAskPip = ask.Substring(6, 1);
                decimal bidPips = Convert.ToDecimal(bid.Substring(4, 3)) / 10;
                decimal askPips = Convert.ToDecimal(ask.Substring(4, 3)) / 10;
                decimal spread = askPips - bidPips;

                forexInstruments.Add(new ForexInstrument
                {
                    QuoteName = quoteName, Spread = spread, BidFirstPart = bidFirstPart, BidSecondPart = bidSecondPart, FractionalBidPip = fractionalBidPip
                    ,AskFirstPart = askFirstPart, AskSecondPart = askSecondPart, FractionalAskPip = fractionalAskPip
                });
            }

            Instruments.ClearAndAddRange(forexInstruments);
            HasQuotesLoaded = true;
        }

        #endregion
    }
}
