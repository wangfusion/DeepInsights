using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    public interface IForexLivePricesService
    {
        Task<string> GetLiveForexPricesJson(IEnumerable<string> quoteNames);
    }
}
