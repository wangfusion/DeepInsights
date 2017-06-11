using DeepInsights.Components.HistoricalPrices.Models;
using DeepInsights.Services;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Constants;
using DeepInsights.Shell.Infrastructure.Events;
using DeepInsights.Shell.Infrastructure.Utilities;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace DeepInsights.Components.HistoricalPrices.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HistoricalPricesMainViewModel : BindableBase, IDisposable
    {
        #region Private Fields

        private string _YAxisLabel;
        private string _XAxisLabel;
        private string _InstrumentName;
        private ModuleStatus _ModuleStatus = new ModuleStatus();

        private readonly IForexHistoricalPricesService _ForexHistoricalPricesService;
        private readonly IEventAggregator _EventAggregator;
        private ImageSource positiveDynamic = new BitmapImage(new Uri(ImageConstants.ArrowUp));
        private ImageSource negativeDynamic = new BitmapImage(new Uri(ImageConstants.ArrowDown));
        private ImageSource zeroDynamic = new BitmapImage(new Uri(ImageConstants.ZeroDynamic));

        #endregion

        #region Constructor

        [ImportingConstructor]
        public HistoricalPricesMainViewModel(IForexHistoricalPricesService forexHistoricalPricesService, IEventAggregator eventAggregator)
        {
            if (forexHistoricalPricesService == null) throw new ArgumentNullException("forexHistoricalPricesService");

            _ForexHistoricalPricesService = forexHistoricalPricesService;
            _EventAggregator = eventAggregator;

            InitializeProperties();
            SubscribeToEvents();
        }

        #endregion

        #region Properties

        public string ModuleHeader
        {
            get { return "Chart"; }
        }

        public ModuleStatus ModuleStatus
        {
            get { return _ModuleStatus; }
            set { SetProperty(ref _ModuleStatus, value); }
        }

        public RangeObservableCollection<Candle> Candles
        {
            get;
            set;
        }

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

        public string InstrumentName
        {
            get { return _InstrumentName; }
            set
            {
                if (_InstrumentName != value)
                {
                    SetProperty(ref _InstrumentName, value);
                    OnPropertyChanged(() => InstrumentName);
                }
            }
        }

        #endregion

        #region Private Methods

        private void InitializeProperties()
        {
            Candles = new RangeObservableCollection<Candle>();
            YAxisLabel = "Price";
            XAxisLabel = "Granularity: " + CandlestickConstants.Day;
            ModuleStatus.HasErrors = true;
        }

        private void SubscribeToEvents()
        {
            _EventAggregator.GetEvent<InstrumentChangedEvent>().Subscribe(ChangeChartInstrument);
        }

        private void UnsubscribeFromEvents()
        {
            _EventAggregator.GetEvent<InstrumentChangedEvent>().Unsubscribe(ChangeChartInstrument);
        }

        private async Task GetCandlestickData(string quoteName)
        {
            try
            {
                //System.Diagnostics.Debugger.Break();
                string candlesJson = await _ForexHistoricalPricesService.GetCandleSticksData(quoteName, CandlestickConstants.AskCandles, CandlestickConstants.Day, DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
                dynamic candlesResult = JsonConvert.DeserializeObject(candlesJson);
                InstrumentName = quoteName;

                Candle lastCandle = null;
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

                    var currentCandle = new Candle(open, high, low, close, time, volume);
                    if (lastCandle != null)
                    {
                        currentCandle.CandleTooltip.OpenDynamic = GetCandleDynamic(lastCandle.Open, currentCandle.Open).ImageSource;
                        currentCandle.CandleTooltip.CloseDynamic = GetCandleDynamic(lastCandle.Close, currentCandle.Close).ImageSource;
                        currentCandle.CandleTooltip.HighDynamic = GetCandleDynamic(lastCandle.High, currentCandle.High).ImageSource;
                        currentCandle.CandleTooltip.LowDynamic = GetCandleDynamic(lastCandle.Low, currentCandle.Low).ImageSource;
                        currentCandle.CandleTooltip.OpenFontBrush = GetCandleDynamic(lastCandle.Open, currentCandle.Open).Brush;
                        currentCandle.CandleTooltip.CloseFontBrush = GetCandleDynamic(lastCandle.Close, currentCandle.Close).Brush;
                        currentCandle.CandleTooltip.HighFontBrush = GetCandleDynamic(lastCandle.High, currentCandle.High).Brush;
                        currentCandle.CandleTooltip.LowFontBrush = GetCandleDynamic(lastCandle.Low, currentCandle.Low).Brush;
                    }
                    candles.Add(lastCandle);
                    lastCandle = currentCandle;
                }

                Candles.ClearAndAddRange(candles);
                ModuleStatus.IsLoaded = true;
                ModuleStatus.HasErrors = false;
            }
            catch (Exception)
            {
                ModuleStatus.HasErrors = true;
            }
        }

        private CandleDynamic GetCandleDynamic(decimal previousValue, decimal currentValue)
        {
            return previousValue < currentValue
                    ? new CandleDynamic(new SolidColorBrush(Color.FromArgb(255, 63, 171, 0)), positiveDynamic)
                    : previousValue > currentValue ? new CandleDynamic(new SolidColorBrush(Color.FromArgb(255, 213, 50, 35)), negativeDynamic)
                                                   : new CandleDynamic(new SolidColorBrush(Color.FromArgb(255, 161, 161, 161)), zeroDynamic);
        }

        private async void ChangeChartInstrument(string quoteName)
        {
            await GetCandlestickData(quoteName);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }

        #endregion
    }
}
