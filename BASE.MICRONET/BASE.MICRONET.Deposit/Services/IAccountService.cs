using BASE.MICRONET.Deposit.DTOs;
using BASE.MICRONET.Deposit.Models;
using System.Threading.Tasks;

namespace BASE.MICRONET.Deposit.Services
{
    public interface IAccountService
    {
        Task<bool> DepositAccount(AccountRequest request);
        bool DepositReverse(Transaction request);
        bool Execute(Transaction request);
    }
}
