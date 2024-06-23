using FacadeMicroservice.Core.Services;

namespace FacadeMicroservice.Logic.Services
{
    public class ConvertCurrency : IConvertCurrency
    {
        private readonly IExchangeRateCache _cache;
        public ConvertCurrency(IExchangeRateCache cache)
        {
            _cache = cache;    
        }

        public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var rates = await _cache.GetExchangeRates();

            if (rates == null)
            {
                throw new ArgumentNullException("Error getting exchange rates");
            }

            if (!rates.ContainsKey(fromCurrency) || !rates.ContainsKey(toCurrency))
            {
                throw new ArgumentNullException("Unrecognized currency");
            }

            var rate = rates[toCurrency] / rates[fromCurrency];
            var convertedAmount = amount * rate;
            return convertedAmount;
        }
    }
}
