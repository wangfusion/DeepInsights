using DeepInsights.Shell.Infrastructure;
using System.Net;

namespace DeepInsights.Services
{
    public class ForexServicesBase
    {
        protected void SetAuthorizationHeader(WebClient webClient)
        {
            webClient.Headers.Add("Authorization", "Bearer " + ApplicationConstants.FX_TOKEN);
        }
    }
}
