namespace TFNValidate.Services
{
    public interface ILinkedValueChecker
    {
        public bool AreLinkedValuesOverThreshold(string[] values, int maxLinkedCount);
    }
}