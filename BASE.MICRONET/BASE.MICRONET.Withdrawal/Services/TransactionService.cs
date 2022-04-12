using BASE.MICRONET.Withdrawal.Models;
using BASE.MICRONET.Withdrawal.Repositories;

namespace BASE.MICRONET.Withdrawal.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ContextDatabase _contextDatabase;

        public TransactionService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public Transaction Withdrawal(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }

        public Transaction WithdrawalReverse(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }
    }
}
