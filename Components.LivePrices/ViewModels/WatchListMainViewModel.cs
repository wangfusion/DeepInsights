using DeepInsights.Components.WatchList.Models;
using DeepInsights.Services;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Events;
using DeepInsights.Shell.Infrastructure.Utilities;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeepInsights.Components.WatchList.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class WatchListMainViewModel : BindableBase
    {
        #region Private Fields

        private ModuleStatus _ModuleStatus = new ModuleStatus();
        private Instrument _SelectedInstrument;
        private QuoteSelectionNotification _QuoteSelectionNotification;
        private string _InteractionResultMessage;
        private readonly IForexWatchListService _ForexWatchListService;
        private readonly IEventAggregator _EventAggregator;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public WatchListMainViewModel(IForexWatchListService forexWatchListService, IEventAggregator eventAggregator)
        {
            InitializeCommands();
            Instruments = new RangeObservableCollection<Instrument>();
            QuoteSelectionRequest = new InteractionRequest<QuoteSelectionNotification>();

            if (forexWatchListService == null) throw new ArgumentNullException("forexWatchListService");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            _ForexWatchListService = forexWatchListService;
            _EventAggregator = eventAggregator;
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

        public ModuleStatus ModuleStatus
        {
            get { return _ModuleStatus; }
            set { SetProperty(ref _ModuleStatus, value); }
        }

        public Instrument SelectedInstrument
        {
            get { return _SelectedInstrument; }
            set
            {
                if (SetProperty(ref _SelectedInstrument, value))
                {
                    UpdateChart(value.QuoteName);
                }
            }
        }

        public InteractionRequest<QuoteSelectionNotification> QuoteSelectionRequest
        {
            get;
            private set;
        }

        public string InteractionResultMessage
        {
            get { return _InteractionResultMessage; }
            set { SetProperty(ref _InteractionResultMessage, value); }
        }

        #endregion

        #region Commands

        public ICommand ViewLoadedCommand
        {
            get;
            private set;
        }

        public DelegateCommand RaiseQuotesSelectionCommand
        {
            get;
            private set;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            ViewLoadedCommand = new DelegateCommand(ViewLoaded);
            RaiseQuotesSelectionCommand = new DelegateCommand(RaiseQuotesSelection, CanRaiseQuotesSelection);
        }

        private async void ViewLoaded()
        {
            await InitializeWatchList();
            await InitializeQuotes();
        }

        private async Task InitializeWatchList()
        {
            var quoteNames = new List<string>
            {
                CurrencyConstants.AUD_CAD, CurrencyConstants.AUD_CHF, CurrencyConstants.AUD_NZD, CurrencyConstants.AUD_USD,
                CurrencyConstants.CAD_CHF, CurrencyConstants.EUR_AUD, CurrencyConstants.EUR_CAD,
                CurrencyConstants.EUR_CHF, CurrencyConstants.EUR_GBP, CurrencyConstants.EUR_NZD
            };

            try
            {
                IEnumerable<Instrument> forexInstruments = await FetchWatchList(quoteNames);
                Instruments.ClearAndAddRange(forexInstruments);
                ModuleStatus.IsLoaded = true;
                RaiseQuotesSelectionCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exception)
            {
                ModuleStatus.HasErrors = true;
                ModuleStatus.ErrorMessage = exception.Message;
            }
        }

        private async Task<IEnumerable<Instrument>> FetchWatchList(IList<string> quoteNames)
        {
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
                    QuoteName = quoteName,
                    Spread = spread,
                    BidFirstPart = bidFirstPart,
                    BidSecondPart = bidSecondPart,
                    FractionalBidPip = fractionalBidPip
                    ,
                    AskFirstPart = askFirstPart,
                    AskSecondPart = askSecondPart,
                    FractionalAskPip = fractionalAskPip
                });
            }

            return forexInstruments;
        }

        private void UpdateChart(string quoteName)
        {
            if (_EventAggregator != null)
            {
                _EventAggregator.GetEvent<InstrumentChangedEvent>().Publish(quoteName);
            }
        }

        private async Task InitializeQuotes()
        {
            _QuoteSelectionNotification = new QuoteSelectionNotification();
            _QuoteSelectionNotification.Title = "Quotes";
            try
            {
                IEnumerable<string> quotes = await GetQuoteNames();
                foreach (string quote in quotes)
                {
                    _QuoteSelectionNotification.Quotes.Add(quote);
                }
            }
            catch (Exception ex)
            {
                // To refactor into messagebox service
                System.Windows.MessageBox.Show("There was an issue getting the full list of quotes: " + ex.Message);
            }
        }

        private async Task<IEnumerable<string>> GetQuoteNames()
        {
            string forexQuotesJson = await _ForexWatchListService.GetAvailableForexQuotes();
            dynamic instrumentsResult = JsonConvert.DeserializeObject(forexQuotesJson);

            var quotes = new List<string>();
            foreach (dynamic instrument in instrumentsResult.instruments)
            {
                string name = instrument.name;
                quotes.Add(name);
            }

            return quotes;
        }

        private void RaiseQuotesSelection()
        {
            InteractionResultMessage = "";
            QuoteSelectionRequest.Raise(_QuoteSelectionNotification,
                returned =>
                {
                    if (returned != null && returned.Confirmed && returned.SelectedQuote != null)
                    {
                        AddNewQuote(returned.SelectedQuote);
                        InteractionResultMessage = "The user selected: " + returned.SelectedQuote;
                    }
                });
        }

        private bool CanRaiseQuotesSelection()
        {
            return ModuleStatus.IsLoaded == true;
        }

        private async void AddNewQuote(string quoteName)
        {
            var quotes = new List<string> { quoteName };
            IEnumerable<Instrument> forexInstruments = await FetchWatchList(quotes);
            Instruments.AddRange(forexInstruments);
        }

        #endregion
    }
}
