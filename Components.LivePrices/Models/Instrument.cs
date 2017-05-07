namespace DeepInsights.Components.WatchList.Models
{
    public abstract class Instrument
    {
        public decimal Spread { get; set; }

        public string QuoteName { get; set; }

        public string BidFirstPart { get; set; }

        public string BidSecondPart { get; set; }

        public string FractionalBidPip { get; set; }

        public string AskFirstPart { get; set; }

        public string AskSecondPart { get; set; }

        public string FractionalAskPip { get; set; }
    }
}
