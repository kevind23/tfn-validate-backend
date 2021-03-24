using Microsoft.AspNetCore.Mvc;
using TFNValidate.API.Models;
using TFNValidate.Services;

namespace TFNValidate.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateController : ControllerBase
    {
        private readonly ITfnValidator _validator;

        public ValidateController(ITfnValidator validator)
        {
            _validator = validator;
        }

        [HttpGet("{taxFileNumber}")]
        public ResultDTO GetIsValid(string taxFileNumber)
        {
            var isValid = _validator.Validate(taxFileNumber);
            return new ResultDTO(isValid);
        }
    }
}