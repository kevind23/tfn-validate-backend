using Moq;
using NUnit.Framework;
using TFNValidate.API.Controllers;
using TFNValidate.API.Models;
using TFNValidate.Services;

namespace TFNValidate.API.Tests
{
    [TestFixture]
    public class ValidateControllerTests
    {
        [TestCase(true, true, null)]
        [TestCase(false, false, null)]
        public void TestGetIsValid(bool validatorResult, bool expectedResult, string expectedError)
        {
            var taxFileNumber = "1234";
            var validatorMock = new Mock<ITfnValidator>();
            validatorMock.Setup(p => p.Validate(taxFileNumber)).Returns(validatorResult);

            var controller = new ValidateController(validatorMock.Object);
            var result = controller.GetIsValid(taxFileNumber);
            
            Assert.AreEqual(expectedResult, result.result);
            Assert.AreEqual(expectedError, result.error);
            validatorMock.VerifyAll();
        }
    }
}
