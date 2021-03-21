using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TFNValidate.API.Models;
using TFNValidate.API.Services;
using TFNValidate.Services;

namespace TFNValidate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateController : ControllerBase
    {
        private readonly ILogger<ValidateController> _logger;
        private readonly ITFNValidator _validator;
        private readonly IAttemptService _attemptService;

        public ValidateController(ILogger<ValidateController> logger, ITFNValidator validator, IAttemptService attemptService)
        {
            _logger = logger;
            _validator = validator;
            _attemptService = attemptService;
        }

        [HttpGet("{taxFileNumber}")]
        public async Task<ResultDTO> GetIsValidAsync(int taxFileNumber)
        {
            await this._attemptService.ClearOldAttempts();
            await this._attemptService.SaveThisAttempt(taxFileNumber);
            var exceedsLinkedThreshold = this._attemptService.AreLinkedAttemptsOverThreshold(taxFileNumber, 3);
            if (exceedsLinkedThreshold)
            {
                return new ResultDTO("Too many similar requests, please try again later.");
            }
            var isValid = this._validator.Validate(taxFileNumber);
            return new ResultDTO(isValid);
        }
    }
}
