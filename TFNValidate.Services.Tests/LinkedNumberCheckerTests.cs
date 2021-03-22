using NUnit.Framework;
using TFNValidate.Services.Implementation;

namespace TFNValidate.Services.Tests
{
    [TestFixture]
    public class LinkedNumberCheckerTests
    {
        [TestCase(123456789, new int[] { }, 3, false)]
        [TestCase(123456789, new[] {123456789, 123456789}, 3, true)]
        [TestCase(123456789, new[] {12349999, 56781111}, 3, true)]
        [TestCase(123456789, new[] {12349999, 22221111}, 3, false)]
        [TestCase(123456789, new[] {123459876, 443459871}, 3, true)]
        public void TestAreLinkedNumbersOverThreshold(int currentNumber, int[] previousNumbers, int maxLinkedCount,
            bool isOverThreshold)
        {
            var checker = new LinkedNumberChecker();
            Assert.That(checker.AreLinkedNumbersOverThreshold(currentNumber, previousNumbers, maxLinkedCount),
                Is.EqualTo(isOverThreshold));
        }
    }
}