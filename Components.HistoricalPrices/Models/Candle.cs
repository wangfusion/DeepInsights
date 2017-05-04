using System;

namespace DeepInsights.Components.HistoricalPrices.Models
{
    public class Candle
    {
        public Candle(decimal open, decimal high, decimal low, decimal close, DateTime time, int volume)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Time = time;
            Volume = volume;
        }

        public decimal Open { get; private set; }

        public decimal High { get; private set; }

        public decimal Low { get; private set; }

        public decimal Close { get; private set; }

        public DateTime Time { get; private set; }

        public int Volume { get; private set; }
    }
}
