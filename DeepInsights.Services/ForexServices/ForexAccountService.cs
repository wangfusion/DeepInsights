using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Utilities;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;

namespace DeepInsights.Services.ForexServices
{
    [Export(typeof(IForexAccountService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForexAccountService : IForexAccountService
    {
        private readonly IHttpUtilities _HttpUtilities;

        [ImportingConstructor]
        public ForexAccountService(IHttpUtilities httpUtilities)
        {
            httpUtilities.ThrowIfNull("httpUtilities");

            _HttpUtilities = httpUtilities;
        }

        public async Task<string> GetAccountData()
        {
            Uri fullUri = new Uri(ApplicationConstants.FX_URL + string.Format(ApplicationConstants.FX_ACCOUNT_ENDPOINT, ApplicationConstants.FX_ACCOUNTID));

            return await _HttpUtilities.GetStringAsync(fullUri);
        }
    }
}
