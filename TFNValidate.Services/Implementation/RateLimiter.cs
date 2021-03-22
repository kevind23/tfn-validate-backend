using System.Threading.Tasks;
using TFNValidate.Persistence;

namespace TFNValidate.Services.Implementation
{
    public class RateLimiter : IRateLimiter
    {
        private readonly IAttemptRepository _repository;
        private readonly ILinkedNumberChecker _linkedNumberChecker;

        public RateLimiter(IAttemptRepository repository, ILinkedNumberChecker linkedNumberChecker)
        {
            _repository = repository;
            _linkedNumberChecker = linkedNumberChecker;
        }

        public async Task<bool> ShouldDenyRequest(int requestedTaxFileNumber, int maxAttempts, int maxTimeMilliseconds)
        {
            await _repository.ClearOldAttempts(maxTimeMilliseconds);
            var previousAttempts = _repository.GetAttempts();
            await _repository.SaveThisAttempt(requestedTaxFileNumber);
            return _linkedNumberChecker.AreLinkedNumbersOverThreshold(requestedTaxFileNumber, previousAttempts, maxAttempts);
        }
    }
}
