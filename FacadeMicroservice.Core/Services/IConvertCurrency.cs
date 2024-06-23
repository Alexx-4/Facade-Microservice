

namespace FacadeMicroservice.Core.Services
{
    public interface IConvertCurrency
    {
        Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
    }
}
