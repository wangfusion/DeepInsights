using DeepInsights.Shell.Infrastructure;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;

namespace DeepInsights.Services.ForexServices
{
    [Export(typeof(IForexAccountService))]
    public class ForexAccountService : ForexServicesBase, IForexAccountService
    {
        public async Task<string> GetAccountData()
        {
            using (var webClient = new WebClient())
            {
                SetAuthorizationHeader(webClient);

                Uri endPoint = new Uri(ApplicationConstants.FX_URL + string.Format(ApplicationConstants.FX_ACCOUNT_ENDPOINT, ApplicationConstants.FX_ACCOUNTID));
                return await webClient.DownloadStringTaskAsync(endPoint);
            }
        }
    }
}
