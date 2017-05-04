using System;
using System.Windows.Media;

namespace DeepInsights.Components.HistoricalPrices.Models
{
    public class CandleTooltip
    {
        public Candle Owner { get; set; }

        public ImageSource HighDynamic { get; set; }

        public ImageSource LowDynamic { get; set; }

        public ImageSource OpenDynamic { get; set; }

        public ImageSource CloseDynamic { get; set; }

        public Brush HighFontBrush { get; set; }

        public Brush LowFontBrush { get; set; }

        public Brush OpenFontBrush { get; set; }

        public Brush CloseFontBrush { get; set; }
    }
}