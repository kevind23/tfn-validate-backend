using System;
using System.Linq;
using System.Threading.Tasks;
using TFNValidate.Persistence.Models;

namespace TFNValidate.Persistence.Implementation
{
    public class AttemptRepository : IAttemptRepository
    {
        private readonly AttemptContext _context;

        public AttemptRepository(AttemptContext context)
        {
            _context = context;
        }

        public async Task ClearOldAttempts(int maximumAgeMilliseconds)
        {
            var oldAttemptsToRemove = _context.Attempts
                .Where(Attempt => (DateTime.Now - Attempt.AttemptTime).TotalMilliseconds > maximumAgeMilliseconds)
                .ToArray();
            _context.RemoveRange(oldAttemptsToRemove);
            await _context.SaveChangesAsync();
        }

        public int[] GetAttempts()
        {
            return _context.Attempts.Select(Attempt => Attempt.TaxFileNumber).ToArray();
        }

        public async Task SaveThisAttempt(int taxFileNumber)
        {
            var attempt = new Attempt
            {
                TaxFileNumber = taxFileNumber,
                AttemptTime = DateTime.Now
            };
            _context.Add(attempt);
            await _context.SaveChangesAsync();
        }
    }
}