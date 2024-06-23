using FacadeMicroservice.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacadeMicroservice.Api.Controllers
{
    [ApiController]
    [Route("/convert")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly IConvertCurrency _convert;

        public CurrencyConverterController(IConvertCurrency convert)
        {
            _convert = convert;
        }

        [HttpGet]
        public async Task<IActionResult> ConvertCurrency([FromQuery] decimal amount, [FromQuery] string from, [FromQuery] string to)
        {
            try
            {
                if (amount <= 0 || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                {
                    return BadRequest("You must provide the amount, from, and to parameters");
                }

                var convertedAmount = await _convert.ConvertAsync(amount, from.ToUpper(), to.ToUpper());

                return Ok(convertedAmount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
