namespace Fin.Api.Model
{
    public class CreateTransaction
    {
        public long AccountId { get; set; }

        public decimal Amount { get; set; }

        public string MessageType { get; set; }

        public string Origin { get; set; }

        public string TransactionId { get; set; }
    }
}
