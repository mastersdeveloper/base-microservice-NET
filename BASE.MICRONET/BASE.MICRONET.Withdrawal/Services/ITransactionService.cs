using BASE.MICRONET.Withdrawal.Models;

namespace BASE.MICRONET.Withdrawal.Services
{
    public interface ITransactionService
    {
        Transaction Withdrawal(Transaction transaction);
        Transaction WithdrawalReverse(Transaction transaction);
    }
}
