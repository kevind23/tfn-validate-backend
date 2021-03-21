using System.Threading.Tasks;

namespace TFNValidate.API.Services
{
    public interface IAttemptService
    {
        public Task ClearOldAttempts();
        public bool AreLinkedAttemptsOverThreshold(int taxFileNumber, int maxLinkedAttempts);
        public Task SaveThisAttempt(int taxFileNumber);
    }
}
