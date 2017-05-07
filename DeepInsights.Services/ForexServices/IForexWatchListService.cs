using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    public interface IForexWatchListService
    {
        Task<string> GetLiveForexPricesJson(IEnumerable<string> quoteNames);
    }
}
