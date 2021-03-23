using NUnit.Framework;
using TFNValidate.Services.Implementation;

namespace TFNValidate.Services.Tests
{
    [TestFixture]
    public class LinkedNumberCheckerTests
    {
        [TestCase(new int[] { 123456789 }, 2, false)]
        [TestCase(new int[] { 123456789, 123456789 }, 2, false)]
        [TestCase(new[] { 123456789, 123456789, 123456789 }, 2, true)]
        [TestCase(new[] { 123456789, 12349999, 56781111 }, 2, true)]
        [TestCase(new[] { 123456789, 12349999, 22221111 }, 2, false)]
        [TestCase(new[] { 123456789, 123459876, 443459871 }, 2, true)]
        public void TestAreLinkedNumbersOverThreshold(int[] numbers, int maxLinkedCount,
            bool isOverThreshold)
        {
            var checker = new LinkedNumberChecker();
            Assert.That(checker.AreLinkedNumbersOverThreshold(numbers, maxLinkedCount),
                Is.EqualTo(isOverThreshold));
        }
    }
}