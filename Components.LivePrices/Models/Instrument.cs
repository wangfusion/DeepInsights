using System;

namespace DeepInsights.Components.LivePrices.Models
{
    public abstract class Instrument
    {
        public double Spread { get; set; }
        public string QuoteName { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double LowestBid { get; set; }
        public double HighestAsk { get; set; }

    }
}
