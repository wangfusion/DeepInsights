using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public static class NameValueCollectionExtensions
    {
        public static string AsEncodedQueryString(this NameValueCollection collection)
        {
            return string.Join("&", (collection.Cast<string>().
                Select(name => string.Concat(name, "=", HttpUtility.UrlEncode(collection[name])))).ToArray());
        }
    }
}
