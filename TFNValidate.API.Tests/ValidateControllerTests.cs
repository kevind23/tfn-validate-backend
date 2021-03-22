using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TFNValidate.Controllers;
using TFNValidate.Services;

namespace TFNValidate.API.Tests
{
    public class ValidateControllerTests
    {
        [Test]
        public async Task TestGetIsValidAsync_NotOverRateLimit_ReturnsValue()
        {
            var taxFileNumber = 12345;
            var expectedMaxLinkedRequests = 3;
            var expectedMaxAgeMilliseconds = 30000;
            var isValid = true;
            
            var mockValidator = new Mock<ITFNValidator>();
            mockValidator.Setup(p => p.Validate(taxFileNumber)).Returns(isValid);

            var mockRateLimiter = new Mock<IRateLimiter>();
            mockRateLimiter.Setup(p => p.ShouldDenyRequest(taxFileNumber, expectedMaxLinkedRequests, expectedMaxAgeMilliseconds)).ReturnsAsync(false);

            var controller = new ValidateController(mockValidator.Object, mockRateLimiter.Object);

            var result = await controller.GetIsValidAsync(taxFileNumber);
            Assert.That(result.result, Is.EqualTo(isValid));
            Assert.That(result.error, Is.Null);
            mockValidator.VerifyAll();
            mockRateLimiter.VerifyAll();
        }

        [Test]
        public async Task TestGetIsValidAsync_IsOverRateLimit_ReturnsError()
        {
            var taxFileNumber = 12345;
            var expectedMaxLinkedRequests = 3;
            var expectedMaxAgeMilliseconds = 30000;

            var mockRateLimiter = new Mock<IRateLimiter>();
            mockRateLimiter.Setup(p => p.ShouldDenyRequest(taxFileNumber, expectedMaxLinkedRequests, expectedMaxAgeMilliseconds)).ReturnsAsync(true);

            var mockValidator = new Mock<ITFNValidator>();

            var controller = new ValidateController(mockValidator.Object, mockRateLimiter.Object);

            var result = await controller.GetIsValidAsync(taxFileNumber);
            Assert.That(result.result, Is.EqualTo(false));
            Assert.That(result.error, Is.EqualTo("Too many similar requests, please try again later."));
            mockValidator.VerifyAll();
            mockRateLimiter.VerifyAll();
        }
    }
}