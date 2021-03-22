using System.Threading.Tasks;

namespace TFNValidate.Services
{
    public interface IRateLimiter
    {
        public Task<bool> ShouldDenyRequest(int requestedTaxFileNumber, int maxAttempts, int timeToCheck);
    }
}