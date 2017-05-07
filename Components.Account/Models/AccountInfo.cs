using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeepInsights.Components.Account.Models
{
    public class AccountInfo
    {
        [JsonProperty(PropertyName ="currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "marginRate")]
        public decimal MarginRate { get; set; }

        [JsonProperty(PropertyName = "hedgingEnabled")]
        public bool HedgingEnabled { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public decimal Balance { get; set; }

        [JsonProperty(PropertyName = "openTradeCount")]
        public int OpenTradeCount { get; set; }

        [JsonProperty(PropertyName = "openPositionCount")]
        public int OpenPositionCount { get; set; }

        [JsonProperty(PropertyName = "pendingOrderCount")]
        public int PendingOrderCount { get; set; }

        [JsonProperty(PropertyName = "pl")]
        public decimal ProfitAndLoss { get; set; }

        [JsonProperty(PropertyName = "resettablePL")]
        public decimal ResettablePL { get; set; }

        [JsonProperty(PropertyName = "financing")]
        public decimal Financing { get; set; }

        [JsonProperty(PropertyName = "commission")]
        public decimal Commission { get; set; }

        [JsonProperty(PropertyName = "orders")]
        public JArray Orders { get; set; }

        [JsonProperty(PropertyName = "positions")]
        public JArray Positions { get; set; }

        [JsonProperty(PropertyName = "trades")]
        public JArray Trades { get; set; }

        [JsonProperty(PropertyName = "unrealizedPL")]
        public decimal UnrealizedPL { get; set; }

        [JsonProperty(PropertyName = "NAV")]
        public decimal NetAssetValue { get; set; }

        [JsonProperty(PropertyName = "marginUsed")]
        public decimal MarginUsed { get; set; }

        [JsonProperty(PropertyName = "marginAvailable")]
        public decimal MarginAvailable { get; set; }

        [JsonProperty(PropertyName = "positionValue")]
        public decimal PositionValue { get; set; }

        //[JsonProperty(PropertyName = "marginCloseoutUnrealizedPL")]
        //public decimal MarginCloseoutUnrealizedPL { get; set; }

        //[JsonProperty(PropertyName = "marginCloseoutNAV")]
        //public decimal MarginCloseoutNAV { get; set; }

        //[JsonProperty(PropertyName = "marginCloseoutMarginUsed")]
        //public decimal MarginCloseoutMarginUsed { get; set; }

        //[JsonProperty(PropertyName = "marginCloseoutPositionValue")]
        //public decimal MarginCloseoutPositionValue { get; set; }

        //[JsonProperty(PropertyName = "marginCloseoutPercent")]
        //public decimal MarginCloseoutPercent { get; set; }

        [JsonProperty(PropertyName = "withdrawalLimit")]
        public decimal WithdrawalLimit { get; set; }

        [JsonProperty(PropertyName = "marginCallMarginUsed")]
        public decimal MarginCallMarginUsed { get; set; }

        [JsonProperty(PropertyName = "marginCallPercent")]
        public decimal MarginCallPercent { get; set; }
    }
}
