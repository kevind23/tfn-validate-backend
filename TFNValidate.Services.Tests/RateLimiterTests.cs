using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TFNValidate.Persistence;
using TFNValidate.Services.Implementation;

namespace TFNValidate.Services.Tests
{
    public class RateLimiterTests
    {
        [Test]
        public async Task TestShouldDenyRequest()
        {
            {
                var taxFileNumber = 1234;
                var maxAttempts = 10;
                var maxTimeMilliseconds = 5234;
                var mockRepository = new Mock<IAttemptRepository>();
                var attempts = new[] {3456, 6789};
                mockRepository.Setup(p => p.ClearOldAttempts(maxTimeMilliseconds)).Verifiable();
                mockRepository.Setup(p => p.GetAttempts()).Returns(attempts);
                mockRepository.Setup(p => p.SaveThisAttempt(taxFileNumber)).Verifiable();

                var mockChecker = new Mock<ILinkedNumberChecker>();
                var expectedOutput = true;
                mockChecker.Setup(p => p.AreLinkedNumbersOverThreshold(taxFileNumber, attempts, maxAttempts))
                    .Returns(expectedOutput);

                var rateLimiter = new RateLimiter(mockRepository.Object, mockChecker.Object);

                var result = await rateLimiter.ShouldDenyRequest(taxFileNumber, maxAttempts, maxTimeMilliseconds);
                Assert.That(result, Is.EqualTo(expectedOutput));

                mockRepository.VerifyAll();
                mockChecker.VerifyAll();
            }
        }
    }
}