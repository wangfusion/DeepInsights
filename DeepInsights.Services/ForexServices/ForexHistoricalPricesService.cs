using DeepInsights.Shell.Infrastructure;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace DeepInsights.Services.ForexServices
{
    [Export(typeof(IForexHistoricalPricesService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForexHistoricalPricesService : ForexServicesBase, IForexHistoricalPricesService
    {
        public async Task<string> GetCandleSticksData(string instrumentName, string candlePriceType, string candlestickGranularity, DateTime fromWhen, DateTime toWhen)
        {
            Contract.Requires(instrumentName != null);
            Contract.Requires(candlePriceType != null);
            Contract.Requires(candlestickGranularity != null);
            Contract.Requires(fromWhen != null);
            Contract.Requires(toWhen != null);

            using (var webClient = new WebClient())
            {
                SetAuthorizationHeader(webClient);

                webClient.QueryString.Add("price", candlePriceType);
                webClient.QueryString.Add("granularity", candlestickGranularity);
                webClient.QueryString.Add("from", XmlConvert.ToString(fromWhen, XmlDateTimeSerializationMode.Local));
                webClient.QueryString.Add("to", XmlConvert.ToString(toWhen, XmlDateTimeSerializationMode.Local));

                Uri endPoint = new Uri(ApplicationConstants.FX_URL + string.Format(ApplicationConstants.FX_CANDLES_ENDPOINT, instrumentName));
                return await webClient.DownloadStringTaskAsync(endPoint);
            }
        }
    }
}
