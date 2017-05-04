using System;

namespace DeepInsights.Components.LivePrices.Models
{
    public abstract class Instrument
    {
        public decimal Spread { get; set; }
        public string QuoteName { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal LowestBid { get; set; }
        public decimal HighestAsk { get; set; }

    }
}
