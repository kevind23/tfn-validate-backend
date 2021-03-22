using System.Threading.Tasks;
using TFNValidate.Persistence;

namespace TFNValidate.Services
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

        public async Task<bool> ShouldDenyRequest(int requestedTaxFileNumber, int maxAttempts, int timeToCheck)
        {
            await _repository.ClearOldAttempts(timeToCheck);
            var previousAttempts = _repository.GetAttempts();
            await _repository.SaveThisAttempt(requestedTaxFileNumber);
            return _linkedNumberChecker.AreLinkedNumbersOverThreshold(requestedTaxFileNumber, previousAttempts, maxAttempts);
        }
    }
}
