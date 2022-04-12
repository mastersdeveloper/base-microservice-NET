using BASE.MICRONET.Cross.Event.Dir.Commands;

namespace BASE.MICRONET.Deposit.Messages.Commands
{
    public class NotificationCreateCommand : Command
    {
        public NotificationCreateCommand(int idTransaction, decimal amount, string type,
            string messageBody, string address, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            MessageBody = messageBody;
            Address = address;
            AccountId = accountId;
        }

        public int IdTransaction { get; protected set; }
        public decimal Amount { get; protected set; }
        public string Type { get; protected set; }
        public string MessageBody { get; protected set; }
        public string Address { get; protected set; }
        public int AccountId { get; protected set; }

    }
}
