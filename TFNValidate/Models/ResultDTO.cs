namespace TFNValidate.API.Models
{
    public class ResultDTO
    {
        public bool result { get; private set; }
        public string error { get; private set; }

        public ResultDTO (bool result)
        {
            this.result = result;
        }

        public ResultDTO (string error)
        {
            this.error = error;
        }
    }
}
