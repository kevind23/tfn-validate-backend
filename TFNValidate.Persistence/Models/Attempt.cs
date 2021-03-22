using System;

namespace TFNValidate.Persistence.Models
{
    public class Attempt
    {
        public long Id { get; set; }
        public int TaxFileNumber { get; set; }
        public DateTime AttemptTime { get; set; }
    }
}
