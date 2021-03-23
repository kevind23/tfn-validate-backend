using System.Threading.Tasks;
using TFNValidate.Persistence;

namespace TFNValidate.Services.Implementation
{
    public class RateLimiter : IRateLimiter
    {
        private readonly ILinkedNumberChecker _linkedNumberChecker;
        private readonly IAttemptRepository _repository;

        public RateLimiter(IAttemptRepository repository, ILinkedNumberChecker linkedNumberChecker)
        {
            _repository = repository;
            _linkedNumberChecker = linkedNumberChecker;
        }

        public async Task<bool> ShouldDenyRequest(int requestedTaxFileNumber, int maxAttempts, int maxTimeMilliseconds)
        {
            await _repository.ClearOldAttempts(maxTimeMilliseconds);
            await _repository.SaveThisAttempt(requestedTaxFileNumber);
            var attempts = _repository.GetAttempts();
            return _linkedNumberChecker.AreLinkedNumbersOverThreshold(attempts, maxAttempts);
        }
    }
}