using System.Threading.Tasks;

namespace DeepInsights.Services.ForexServices
{
    public interface IForexAccountService
    {
        Task<string> GetAccountData();
    }
}
