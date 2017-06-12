using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Utilities;
using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace DeepInsights.Services.ForexServices
{
    [Export(typeof(IForexHistoricalPricesService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForexHistoricalPricesService : IForexHistoricalPricesService
    {
        private readonly IHttpUtilities _HttpUtilities;

        [ImportingConstructor]
        public ForexHistoricalPricesService(IHttpUtilities httpUtilities)
        {
            httpUtilities.ThrowIfNull("httpUtilities");

            _HttpUtilities = httpUtilities;
        }

        public async Task<string> GetCandleSticksData(string instrumentName, string candlePriceType, string candlestickGranularity, DateTime fromWhen, DateTime toWhen)
        {
            instrumentName.ThrowIfNull("instrumentName");
            candlePriceType.ThrowIfNull("candlePriceType");
            candlestickGranularity.ThrowIfNull("candlestickGranularity");
            fromWhen.ThrowIfNull("fromWhen");
            toWhen.ThrowIfNull("toWhen");

            var queryParameters = new NameValueCollection
            {
                { "price", candlePriceType },
                { "granularity", candlestickGranularity },
                { "from", XmlConvert.ToString(fromWhen, XmlDateTimeSerializationMode.Local) },
                { "to", XmlConvert.ToString(toWhen, XmlDateTimeSerializationMode.Local) }
            };

            string baseUri = ApplicationConstants.FX_URL + string.Format(ApplicationConstants.FX_CANDLES_ENDPOINT, instrumentName);
            Uri fullUri = _HttpUtilities.BuildUri(baseUri, queryParameters);

            return await _HttpUtilities.GetStringAsync(fullUri);
        }
    }
}
