namespace TFNValidate.Services.Models
{
    public class RateLimitConfiguration
    {
        public int MaxAttempts { get; set; }
        public int TimeBetweenAttemptsMilliseconds { get; set; }
    }
}
