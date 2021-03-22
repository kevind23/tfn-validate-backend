using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TFNValidate.Persistence.Implementation;
using TFNValidate.Persistence.Models;

namespace TFNValidate.Persistence.Tests
{
    public class AttemptRepositoryTests
    {
        [Test]
        public async Task TestSaveThisAttempt()
        {
            using (var context = GetAttemptContext("TestSaveThisAttempt"))
            {
                var repository = new AttemptRepository(context);
                var taxFileNumber = 12345678;
                var insertTime = DateTime.Now;
                await repository.SaveThisAttempt(taxFileNumber);
                var attempts = await context.Attempts.ToListAsync();
                Assert.AreEqual(1, attempts.Count);
                Assert.AreEqual(taxFileNumber, attempts[0].TaxFileNumber);
                Assert.That(attempts[0].AttemptTime, Is.EqualTo(insertTime).Within(1).Minutes);
            }
        }

        private AttemptContext GetAttemptContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AttemptContext>().UseInMemoryDatabase(databaseName).Options;
            return new AttemptContext(options);
        }

        [Test]
        public async Task TestGetAttempts()
        {
            using (var context = GetAttemptContext("TestGetAttempts"))
            {
                var repository = new AttemptRepository(context);
                var taxFileNumber1 = 12345678;
                await repository.SaveThisAttempt(taxFileNumber1);
                var taxFileNumber2 = 123456789;
                await repository.SaveThisAttempt(taxFileNumber2);
                var attempts = repository.GetAttempts();
                Assert.AreEqual(2, attempts.Length);
                Assert.AreEqual(taxFileNumber1, attempts[0]);
                Assert.AreEqual(taxFileNumber2, attempts[1]);
            }
        }

        [Test]
        public async Task TestClearOldAttempts()
        {
            using (var context = GetAttemptContext("TestClearOldAttempts"))
            {
                var repository = new AttemptRepository(context);
                var taxFileNumber1 = 12345678;
                await repository.SaveThisAttempt(taxFileNumber1);
                var delayTimeMs = 500;
                await Task.Delay(delayTimeMs);
                var taxFileNumber2 = 123456789;
                await repository.SaveThisAttempt(taxFileNumber2);
                await repository.ClearOldAttempts(delayTimeMs);
                var attempts = repository.GetAttempts();
                Assert.AreEqual(1, attempts.Length);
                Assert.AreEqual(taxFileNumber2, attempts[0]);
            }
        }
    }
}