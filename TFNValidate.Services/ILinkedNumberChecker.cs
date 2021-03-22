namespace TFNValidate.Services
{
    public interface ILinkedNumberChecker
    {
        public bool AreLinkedNumbersOverThreshold(int firstNumber, int[] otherNumbers, int maxLinkedCount);
    }
}
