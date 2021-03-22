namespace TFNValidate.Services
{
    public interface ITFNValidator
    {
        bool Validate(int taxFileNumber);
    }
}