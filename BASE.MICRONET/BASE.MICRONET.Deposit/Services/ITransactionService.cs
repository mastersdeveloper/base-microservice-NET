using BASE.MICRONET.Deposit.Models;

namespace BASE.MICRONET.Deposit.Services
{
    public interface ITransactionService
    {
        Transaction Deposit(Transaction transaction);
        Transaction DepositReverse(Transaction transaction);
    }
}
