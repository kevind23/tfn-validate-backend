namespace TFNValidate.Services
{
    public interface ILinkedNumberChecker
    {
        public bool AreLinkedNumbersOverThreshold(int[] numbers, int maxLinkedCount);
    }
}