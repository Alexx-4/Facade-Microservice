

namespace FacadeMicroservice.Core.Services
{
    public interface IExchangeRateCache
    {
        Task<Dictionary<string, decimal>> GetOrUpdateCacheAsync();
        Task<Dictionary<string, decimal>> GetExchangeRates();
    }
}
