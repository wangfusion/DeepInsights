using System;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    public interface IForexHistoricalPricesService
    {
        Task<string> GetCandleSticksData(string instrumentName, string candlePriceType, string candlestickGranularity, DateTime fromWhen, DateTime toWhen);
    }
}
