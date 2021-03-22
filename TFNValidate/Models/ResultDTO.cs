namespace TFNValidate.API.Models
{
    public class ResultDTO
    {
        public ResultDTO(bool result)
        {
            this.result = result;
        }

        public ResultDTO(string error)
        {
            this.error = error;
        }

        public bool result { get; }
        public string error { get; }
    }
}