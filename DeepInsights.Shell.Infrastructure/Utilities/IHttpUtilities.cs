using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public interface IHttpUtilities
    {
        HttpClient GetHttpClient();
        Uri BuildUri(string root, NameValueCollection queryString);
        Task<string> GetStringAsync(Uri uri);
    }
}
