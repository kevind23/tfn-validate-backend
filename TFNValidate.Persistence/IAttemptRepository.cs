using System.Threading.Tasks;

namespace TFNValidate.Persistence
{
    public interface IAttemptRepository
    {
        public Task ClearOldAttempts(int maximumAgeMilliseconds);
        public int[] GetAttempts();
        public Task SaveThisAttempt(int taxFileNumber);
    }
}
