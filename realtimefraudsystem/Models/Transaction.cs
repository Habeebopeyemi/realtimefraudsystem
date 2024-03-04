namespace realtimefraudsystem.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Country { get; set; }     
        public DateOnly Timestamp { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public long AccountNumber { get; set; }
        public long Status { get; set; }
        public long? DestinationAccountNumber { get; set; }
        public long Transaction_id { get; set; }
    }
}
