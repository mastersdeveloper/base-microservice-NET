using BASE.MICRONET.Deposit.Models;
using BASE.MICRONET.Deposit.Repositories;

namespace BASE.MICRONET.Deposit.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ContextDatabase _contextDatabase;

        public TransactionService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public Transaction Deposit(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }

        public Transaction DepositReverse(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }
    }
}
