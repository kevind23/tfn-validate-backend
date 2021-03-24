using NUnit.Framework;
using TFNValidate.Services.Implementation;

namespace TFNValidate.Services.Tests
{
    [TestFixture]
    public class LinkedValueCheckerTests
    {
        [TestCase(new[] { "123456789" }, 2, false)]
        [TestCase(new[] { "123456789", "123456789" }, 2, false)]
        [TestCase(new[] { "123456789", "123456789", "123456789" }, 2, true)]
        [TestCase(new[] { "123456789", "12349999", "56781111" }, 2, true)]
        [TestCase(new[] { "123456789", "12349999", "22221111" }, 2, false)]
        [TestCase(new[] { "123456789", "123459876", "443459871" }, 2, true)]
        public void TestAreLinkedValuesOverThreshold(string[] values, int maxLinkedCount,
            bool isOverThreshold)
        {
            var checker = new LinkedValueChecker();
            Assert.That(checker.AreLinkedValuesOverThreshold(values, maxLinkedCount),
                Is.EqualTo(isOverThreshold));
        }
    }
}