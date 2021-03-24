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
            object[] oldAttemptsToRemove = _context.Attempts
                .Where(attempt => (DateTime.Now - attempt.AttemptTime).TotalMilliseconds > maximumAgeMilliseconds)
                .ToArray();
            _context.RemoveRange(oldAttemptsToRemove);
            await _context.SaveChangesAsync();
        }

        public string[] GetAttemptsFor(string clientIpAddress)
        {
            return _context.Attempts.Where(attempt => attempt.ClientIpAddress == clientIpAddress)
                .Select(attempt => attempt.TaxFileNumber).ToArray();
        }

        public async Task SaveThisAttempt(string taxFileNumber, string clientIpAddress)
        {
            var attempt = new Attempt
            {
                TaxFileNumber = taxFileNumber,
                AttemptTime = DateTime.Now,
                ClientIpAddress = clientIpAddress
            };
            _context.Add(attempt);
            await _context.SaveChangesAsync();
        }
    }
}