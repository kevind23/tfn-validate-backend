using NUnit.Framework;
using TFNValidate.Services.Implementation;

namespace TFNValidate.Services.Tests
{
    [TestFixture]
    public class TfnValidatorTests
    {
        [TestCase("000000000", true)]
        [TestCase("648188480", true)]
        [TestCase("648188499", true)]
        [TestCase("648188519", true)]
        [TestCase("648188527", true)]
        [TestCase("648188535", true)]
        [TestCase("37118629", true)]
        [TestCase("37118655", true)]
        [TestCase("37118660", true)]
        [TestCase("37118676", true)]
        [TestCase("37118705", true)]
        [TestCase("38593469", true)]
        [TestCase("38593474", true)]
        [TestCase("38593503", true)]
        [TestCase("38593519", true)]
        [TestCase("38593524", true)]
        [TestCase("85655734", true)]
        [TestCase("85655755", true)]
        [TestCase("85655797", true)]
        [TestCase("85655805", true)]
        [TestCase("85655810", true)]
        [TestCase("00000000", true)]
        [TestCase("0000000", false)]
        [TestCase("0000000000", false)]
        public void TestValidate(string taxFileNumber, bool isValid)
        {
            var validator = new TfnValidator();
            Assert.That(validator.Validate(taxFileNumber), Is.EqualTo(isValid));
        }
    }
}