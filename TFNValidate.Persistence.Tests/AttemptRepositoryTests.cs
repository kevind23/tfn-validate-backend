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
            await using var context = GetAttemptContext("TestSaveThisAttempt");
            var repository = new AttemptRepository(context);
            var taxFileNumber = "12345678";
            var clientIpAddress = "192.168.1.1";
            var insertTime = DateTime.Now;
            await repository.SaveThisAttempt(taxFileNumber, clientIpAddress);
            var attempts = await context.Attempts.ToListAsync();
            Assert.AreEqual(1, attempts.Count);
            Assert.AreEqual(taxFileNumber, attempts[0].TaxFileNumber);
            Assert.AreEqual(clientIpAddress, attempts[0].ClientIpAddress);
            Assert.That(attempts[0].AttemptTime, Is.EqualTo(insertTime).Within(1).Minutes);
        }

        private AttemptContext GetAttemptContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AttemptContext>().UseInMemoryDatabase(databaseName).Options;
            return new AttemptContext(options);
        }

        [Test]
        public async Task TestGetAttempts()
        {
            await using var context = GetAttemptContext("TestGetAttempts");
            var repository = new AttemptRepository(context);
            var taxFileNumber1 = "12345678";
            var clientIpAddress = "192.168.1.1";
            await repository.SaveThisAttempt(taxFileNumber1, clientIpAddress);
            var taxFileNumber2 = "123456789";
            await repository.SaveThisAttempt(taxFileNumber2, clientIpAddress);
            var taxFileOtherIp = "12341234";
            await repository.SaveThisAttempt(taxFileOtherIp, "192.168.2.2");
            var attempts = repository.GetAttemptsFor(clientIpAddress);
            Assert.AreEqual(2, attempts.Length);
            Assert.AreEqual(taxFileNumber1, attempts[0]);
            Assert.AreEqual(taxFileNumber2, attempts[1]);
        }

        [Test]
        public async Task TestClearOldAttempts()
        {
            await using var context = GetAttemptContext("TestClearOldAttempts");
            var repository = new AttemptRepository(context);
            var taxFileNumber1 = "12345678";
            var clientIpAddress = "192.168.1.1";
            await repository.SaveThisAttempt(taxFileNumber1, clientIpAddress);
            var delayTimeMs = 500;
            await Task.Delay(delayTimeMs);
            var taxFileNumber2 = "123456789";
            await repository.SaveThisAttempt(taxFileNumber2, clientIpAddress);
            await repository.ClearOldAttempts(delayTimeMs);
            var attempts = repository.GetAttemptsFor(clientIpAddress);
            Assert.AreEqual(1, attempts.Length);
            Assert.AreEqual(taxFileNumber2, attempts[0]);
        }
    }
}