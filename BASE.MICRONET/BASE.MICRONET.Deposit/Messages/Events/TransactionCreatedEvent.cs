using BASE.MICRONET.Cross.Event.Dir.Events;

namespace BASE.MICRONET.Deposit.Messages.Events
{
    public class TransactionCreatedEvent : Event
    {
        public TransactionCreatedEvent(int idTransaction, decimal amount, string type, string creationDate, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            CreationDate = creationDate;
            AccountId = accountId;
        }

        public int IdTransaction { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string CreationDate { get; set; }
        public int AccountId { get; set; }

    }
}
