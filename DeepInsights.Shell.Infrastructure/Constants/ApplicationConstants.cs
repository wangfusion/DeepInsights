namespace DeepInsights.Shell.Infrastructure
{
    public static class ApplicationConstants
    {
        // Base Urls
        public static readonly string FX_URL = "https://api-fxpractice.oanda.com";
        public static readonly string FX_TOKEN = "15b0901cd95ca514e2fdd811b306ae3a-392b29326ae2cf658ad31307aaa94042";

        public static readonly string FX_ACCOUNTID = "101-003-4355710-001";

        // FX EndPoints
        public static readonly string FX_ACCOUNT_ENDPOINT = "/v3/accounts/{0}";
        public static readonly string FX_PRICING_ENDPOINT = "/v3/accounts/101-003-4355710-001/pricing";
        public static readonly string FX_CANDLES_ENDPOINT = "/v3/instruments/{0}/candles";
    }
}
