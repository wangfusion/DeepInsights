using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    [Export(typeof(IHttpUtilities))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class HttpUtilities : IHttpUtilities
    {
        private readonly HttpClient _HttpClient;

        public HttpUtilities()
        {
            // Make sure the Framework don't try to auto-detect proxy settings
            var handler = new HttpClientHandler { UseProxy = false };
            _HttpClient = new HttpClient(handler);
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApplicationConstants.FX_TOKEN);
        }

        public HttpClient GetHttpClient()
        {
            return _HttpClient;
        }

        public Uri BuildUri(string root, NameValueCollection queryString)
        {
            root.ThrowIfNull("root");

            var builder = new UriBuilder(root);
            if (queryString != null)
            {
                builder.Query = string.Join("&", queryString.Cast<string>()
                                                            .Select(name => string.Concat(name, "=", HttpUtility.UrlEncode(queryString[name]))).ToArray());
            }

            return builder.Uri;
        }

        public async Task<string> GetStringAsync(Uri uri)
        {
            return await _HttpClient.GetStringAsync(uri);
        }
    }
}
