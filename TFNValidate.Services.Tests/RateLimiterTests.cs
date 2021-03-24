using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using TFNValidate.Persistence;
using TFNValidate.Services.Implementation;
using TFNValidate.Services.Models;

namespace TFNValidate.Services.Tests
{
    public class RateLimiterTests
    {
        [Test]
        public async Task TestShouldDenyRequest()
        {
            {
                var taxFileNumber = "1234";
                var maxAttempts = 10;
                var clientIpAddress = "192.168.1.1";
                var maxTimeMilliseconds = 5234;
                var mockRepository = new Mock<IAttemptRepository>();
                var attempts = new[] {"3456", "6789"};
                mockRepository.Setup(p => p.ClearOldAttempts(maxTimeMilliseconds)).Verifiable();
                mockRepository.Setup(p => p.GetAttemptsFor(clientIpAddress)).Returns(attempts);
                mockRepository.Setup(p => p.SaveThisAttempt(taxFileNumber, clientIpAddress)).Verifiable();

                var mockChecker = new Mock<ILinkedValueChecker>();
                var expectedOutput = true;
                mockChecker.Setup(p => p.AreLinkedValuesOverThreshold(attempts, maxAttempts))
                    .Returns(expectedOutput);

                var configuration = new RateLimitConfiguration()
                {
                    MaxAttempts = maxAttempts,
                    TimeBetweenAttemptsMilliseconds = maxTimeMilliseconds
                };
                var injectableConfiguration = Options.Create(configuration);

                var rateLimiter = new RateLimiter(mockRepository.Object, mockChecker.Object, injectableConfiguration);

                var result = await rateLimiter.ShouldDenyRequest(taxFileNumber, clientIpAddress);
                Assert.That(result, Is.EqualTo(expectedOutput));

                mockRepository.VerifyAll();
                mockChecker.VerifyAll();
            }
        }
    }
}