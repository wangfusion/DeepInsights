using DeepInsights.Shell.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    [Export(typeof(IForexWatchListService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForexWatchListService : ForexServicesBase, IForexWatchListService
    {
        public async Task<string> GetLiveForexPricesJson(IEnumerable<string> quoteNames)
        {
            string instrumentsQuotes = string.Join(",", quoteNames);

            var queryParameters = new NameValueCollection();
            queryParameters.Add(ForexJsonParameterConstants.Instruments, instrumentsQuotes);
            Uri endPoint = new Uri(ApplicationConstants.FX_URL + ApplicationConstants.FX_PRICING_ENDPOINT);

            return await DownloadJsonAsync(endPoint, queryParameters);
        }

        public async Task<string> GetAvailableForexQuotes()
        {
            Uri endPoint = new Uri(ApplicationConstants.FX_URL + string.Format(ApplicationConstants.FX_INSTRUMENTS_ENDPOINT, ApplicationConstants.FX_ACCOUNTID));

            return await DownloadJsonAsync(endPoint);
        }

        private async Task<string> DownloadJsonAsync(Uri endPoint)
        {
            return await DownloadJsonAsync(endPoint, null);
        }

        private async Task<string> DownloadJsonAsync(Uri endPoint, NameValueCollection queryParameters)
        {
            using (var webClient = new WebClient())
            {
                SetAuthorizationHeader(webClient);
                if (queryParameters != null)
                {
                    webClient.QueryString.Add(queryParameters);
                }

                return await webClient.DownloadStringTaskAsync(endPoint);
            }
        }
    }
}
