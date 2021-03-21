using System;
using System.Linq;
using System.Threading.Tasks;
using TFNValidate.API.Models;
using TFNValidate.Services;

namespace TFNValidate.API.Services
{
    public class AttemptService : IAttemptService
    {
        private readonly AttemptContext _context;

        public AttemptService(AttemptContext context) {
            this._context = context;
        }

        public async Task ClearOldAttempts()
        {
            var maximumAgeSeconds = 30;
            var oldAttemptsToRemove = this._context.Attempts
                .Where(Attempt => (DateTime.Now - Attempt.AttemptTime).TotalSeconds > maximumAgeSeconds)
                .ToArray();
            this._context.RemoveRange(oldAttemptsToRemove);
            await _context.SaveChangesAsync();
        }

        public bool AreLinkedAttemptsOverThreshold(int currentAttempt, int maxLinkedAttempts)
        {
            var previousAttempts = this.GetPreviousAttempts();
            return this.DoCheckLinkedAttemptsAgainstThreshold(currentAttempt, previousAttempts, maxLinkedAttempts);
        }

        private int[] GetPreviousAttempts()
        {
            return this._context.Attempts.Select(Attempt => Attempt.TaxFileNumber).ToArray();
        }

        private bool DoCheckLinkedAttemptsAgainstThreshold(int currentAttempt, int[] recentAttempts, int maxLinkedAttempts)
        {
            return LinkedNumberChecker.AreLinkedNumbersOverThreshold(currentAttempt, recentAttempts, maxLinkedAttempts);
        }

        public async Task SaveThisAttempt(int taxFileNumber)
        {
            Attempt attempt = new Attempt
            {
                TaxFileNumber = taxFileNumber,
                AttemptTime = DateTime.Now
            };
            this._context.Add(attempt);
            await _context.SaveChangesAsync();
        }
    }
}
