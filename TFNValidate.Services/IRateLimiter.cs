using System.Threading.Tasks;

namespace TFNValidate.Services
{
    public interface IRateLimiter
    {
        public Task<bool> ShouldDenyRequest(string requestedTaxFileNumber, string clientIpAddress);
    }
}