using System.Threading.Tasks;

namespace TFNValidate.Persistence
{
    public interface IAttemptRepository
    {
        public Task ClearOldAttempts(int maximumAgeMilliseconds);
        public string[] GetAttemptsFor(string clientIpAddress);
        public Task SaveThisAttempt(string taxFileNumber, string clientIpAddress);
    }
}