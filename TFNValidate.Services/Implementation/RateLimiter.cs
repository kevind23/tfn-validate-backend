using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TFNValidate.Persistence;
using TFNValidate.Services.Models;

namespace TFNValidate.Services.Implementation
{
    public class RateLimiter : IRateLimiter
    {
        private readonly ILinkedValueChecker _linkedValueChecker;
        private readonly IAttemptRepository _repository;
        private readonly IOptions<RateLimitConfiguration> _configuration;

        public RateLimiter(IAttemptRepository repository, ILinkedValueChecker linkedValueChecker,
            IOptions<RateLimitConfiguration> configuration)
        {
            _repository = repository;
            _linkedValueChecker = linkedValueChecker;
            _configuration = configuration;
        }

        public async Task<bool> ShouldDenyRequest(string requestedTaxFileNumber, string clientIpAddress)
        {
            await _repository.ClearOldAttempts(_configuration.Value.TimeBetweenAttemptsMilliseconds);
            await _repository.SaveThisAttempt(requestedTaxFileNumber, clientIpAddress);
            var attempts = _repository.GetAttemptsFor(clientIpAddress);
            return _linkedValueChecker.AreLinkedValuesOverThreshold(attempts, _configuration.Value.MaxAttempts);
        }
    }
}