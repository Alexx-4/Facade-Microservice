using FacadeMicroservice.Core.Services;
using Hangfire;

namespace FacadeMicroservice.Logic.Jobs
{
    [DisableConcurrentExecution(10 * 60 * 60)]
    public class UpdateExchangeRatesCache
    {

        private readonly IExchangeRateCache _cache;

        public UpdateExchangeRatesCache(IExchangeRateCache cache)
        {
            _cache = cache;
        }

        [DisableConcurrentExecution(10 * 60 * 60)]
        public async Task Run()
        {
            await _cache.GetOrUpdateCacheAsync();
        }
    }
}
