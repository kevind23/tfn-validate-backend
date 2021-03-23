using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TFNValidate.API.Models;
using TFNValidate.Services;

namespace TFNValidate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateController : ControllerBase
    {
        private readonly IRateLimiter _rateLimiter;
        private readonly ITFNValidator _validator;

        public ValidateController(ITFNValidator validator, IRateLimiter rateLimiter)
        {
            _validator = validator;
            _rateLimiter = rateLimiter;
        }

        [HttpGet("{taxFileNumber}")]
        public async Task<ResultDTO> GetIsValidAsync(int taxFileNumber)
        {
            var maxAgeMilliseconds = 30 * 1000;
            var maxLinkedRequests = 2;
            var isOverRateLimit =
                await _rateLimiter.ShouldDenyRequest(taxFileNumber, maxLinkedRequests, maxAgeMilliseconds);
            if (isOverRateLimit) return new ResultDTO("Too many similar requests, please try again later.");
            var isValid = _validator.Validate(taxFileNumber);
            return new ResultDTO(isValid);
        }
    }
}