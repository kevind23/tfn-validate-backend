using System;

namespace TFNValidate.Persistence.Models
{
    public class Attempt
    {
        public long Id { get; set; }
        public string TaxFileNumber { get; set; }
        public DateTime AttemptTime { get; set; }
        public string ClientIpAddress { get; set; }
    }
}