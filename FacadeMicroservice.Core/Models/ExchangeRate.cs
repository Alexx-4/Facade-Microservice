

namespace FacadeMicroservice.Core.Models
{
    public class ExchangeRate
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
