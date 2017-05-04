using DeepInsights.Shell.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    [Export(typeof(IForexLivePricesService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForexLivePricesService : IForexLivePricesService
    {
        public async Task<string> GetLiveForexPricesJson(IEnumerable<string> quoteNames)
        {
            string instrumentsQuotes = string.Join(",", quoteNames);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", "Bearer " + ApplicationConstants.FX_TOKEN);
                webClient.QueryString.Add("instruments", instrumentsQuotes);

                return await webClient.DownloadStringTaskAsync(new Uri(ApplicationConstants.FX_URL + ApplicationConstants.FX_PRICING_ENDPOINT));
            }
        }
    }
}
