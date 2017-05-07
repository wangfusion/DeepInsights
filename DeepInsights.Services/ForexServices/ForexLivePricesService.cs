using DeepInsights.Shell.Infrastructure;
using System;
using System.Collections.Generic;
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

            using (var webClient = new WebClient())
            {
                SetAuthorizationHeader(webClient);
                webClient.QueryString.Add("instruments", instrumentsQuotes);

                Uri endPoint = new Uri(ApplicationConstants.FX_URL + ApplicationConstants.FX_PRICING_ENDPOINT);
                return await webClient.DownloadStringTaskAsync(endPoint);
            }
        }
    }
}
