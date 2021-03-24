namespace TFNValidate.Services
{
    public interface ITfnValidator
    {
        bool Validate(string taxFileNumber);
    }
}