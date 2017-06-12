using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    [Export(typeof(IForexWatchListService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForexWatchListService : IForexWatchListService
    {
        private readonly IHttpUtilities _HttpUtilities;

        [ImportingConstructor]
        public ForexWatchListService(IHttpUtilities httpUtilities)
        {
            httpUtilities.ThrowIfNull("httpUtilities");

            _HttpUtilities = httpUtilities;
        }

        public async Task<string> GetLiveForexPricesJson(IEnumerable<string> quoteNames)
        {
            string instrumentsQuotes = string.Join(",", quoteNames);

            var queryParameters = new NameValueCollection();
            queryParameters.Add(ForexJsonParameterConstants.Instruments, instrumentsQuotes);
            string baseUri = ApplicationConstants.FX_URL + ApplicationConstants.FX_PRICING_ENDPOINT;

            return await DownloadJsonAsync(baseUri, queryParameters);
        }

        public async Task<string> GetAvailableForexQuotes()
        {
            string baseUri = ApplicationConstants.FX_URL + string.Format(ApplicationConstants.FX_INSTRUMENTS_ENDPOINT, ApplicationConstants.FX_ACCOUNTID);

            return await DownloadJsonAsync(baseUri);
        }

        private async Task<string> DownloadJsonAsync(string baseUri)
        {
            return await DownloadJsonAsync(baseUri, null);
        }

        private async Task<string> DownloadJsonAsync(string baseUri, NameValueCollection queryParameters)
        {
            Uri fullUri = _HttpUtilities.BuildUri(baseUri, queryParameters);
            return await _HttpUtilities.GetStringAsync(fullUri);
        }
    }
}
