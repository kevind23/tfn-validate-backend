using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TFNValidate.Persistence.Models;
using TFNValidate.Services;

namespace TFNValidate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateController : ControllerBase
    {
        private readonly ITFNValidator _validator;
        private readonly IRateLimiter _rateLimiter;

        public ValidateController(ITFNValidator validator, IRateLimiter rateLimiter)
        {
            _validator = validator;
            _rateLimiter = rateLimiter;
        }

        [HttpGet("{taxFileNumber}")]
        public async Task<ResultDTO> GetIsValidAsync(int taxFileNumber)
        {
            var isOverRateLimit = await _rateLimiter.ShouldDenyRequest(taxFileNumber, 3, 30);
            if (isOverRateLimit)
            {
                return new ResultDTO("Too many similar requests, please try again later.");
            }
            var isValid = _validator.Validate(taxFileNumber);
            return new ResultDTO(isValid);
        }
    }
}
