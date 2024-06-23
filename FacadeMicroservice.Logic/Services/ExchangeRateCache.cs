using FacadeMicroservice.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using FacadeMicroservice.Core.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace FacadeMicroservice.Logic.Services
{
    public class ExchangeRateCache : IExchangeRateCache
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;


        public ExchangeRateCache(IConfiguration configuration,IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
        }

        public async Task<Dictionary<string, decimal>> GetOrUpdateCacheAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_configuration["OpenExchangeUrl"] + _configuration["OpenExchangeAppId"]);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var data = await response.Content.ReadAsStringAsync();
                ExchangeRate exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(data);
                var cacheOptions = new MemoryCacheEntryOptions();
                _cache.Set("exchangeRates", exchangeRate.Rates, cacheOptions);
                return exchangeRate.Rates;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRates()
        {
            if (_cache.TryGetValue("exchangeRates", out Dictionary<string, decimal> rates))
            {
                return rates;
            }

            return await GetOrUpdateCacheAsync();
        }
    }
}
