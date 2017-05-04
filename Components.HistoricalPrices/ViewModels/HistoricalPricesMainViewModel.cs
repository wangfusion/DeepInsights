using DeepInsights.Components.HistoricalPrices.Models;
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
using System.Xml;

namespace DeepInsights.Components.HistoricalPrices.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HistoricalPricesMainViewModel : BindableBase
    {
        #region Private Fields

        private string _YAxisLabel;
        private string _XAxisLabel;
        private bool _HasPricesLoaded;
        private readonly IForexHistoricalPricesService _ForexHistoricalPricesService;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public HistoricalPricesMainViewModel(IForexHistoricalPricesService forexHistoricalPricesService)
        {
            if (forexHistoricalPricesService == null)
            {
                throw new ArgumentNullException("forexHistoricalPricesService");
            }

            _ForexHistoricalPricesService = forexHistoricalPricesService;
            InitializeProperties();
            InitializeCommands();
        }

        #endregion

        #region Properties

        public string YAxisLabel
        {
            get { return _YAxisLabel; }
            set
            {
                if (_YAxisLabel != value)
                {
                    SetProperty(ref _YAxisLabel, value);
                    OnPropertyChanged(() => YAxisLabel);
                }
            }
        }

        public string XAxisLabel
        {
            get { return _XAxisLabel; }
            set
            {
                if (_XAxisLabel != value)
                {
                    SetProperty(ref _XAxisLabel, value);
                    OnPropertyChanged(() => XAxisLabel);
                }
            }
        }

        public bool HasPricesLoaded
        {
            get { return _HasPricesLoaded; }
            set
            {
                if (_HasPricesLoaded != value)
                {
                    SetProperty(ref _HasPricesLoaded, value);
                    OnPropertyChanged(() => HasPricesLoaded);
                }
            }
        }

        public RangeObservableCollection<Candle> Candles
        {
            get;
            set;
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

        private void InitializeProperties()
        {
            Candles = new RangeObservableCollection<Candle>();
            YAxisLabel = "Price";
            XAxisLabel = "Granularity: " + CandlestickConstants.Day;
        }

        private void InitializeCommands()
        {
            ViewLoadedCommand = new DelegateCommand(ViewLoaded);
        }

        private async void ViewLoaded()
        {
            await GetCandlestickData();
        }

        private async Task GetCandlestickData()
        {
            string candlesJson = await _ForexHistoricalPricesService.GetCandleSticksData(CurrencyConstants.EUR_USD, CandlestickConstants.AskCandles, CandlestickConstants.Day, DateTime.Now.AddMonths(-1), DateTime.Now);
            dynamic candlesResult = JsonConvert.DeserializeObject(candlesJson);

            var candles = new List<Candle>();
            foreach (dynamic candle in candlesResult.candles)
            {
                string candleTime = candle.time;
                DateTime time = XmlConvert.ToDateTime(candleTime, XmlDateTimeSerializationMode.Local);
                int volume = candle.volume;
                decimal open = candle.ask.o;
                decimal high = candle.ask.h;
                decimal low = candle.ask.l;
                decimal close = candle.ask.c;

                candles.Add(new Candle(open, high, low, close, time, volume));
            }

            Candles.ClearAndAddRange(candles);
            HasPricesLoaded = true;
        }

        #endregion
    }
}
