using Microsoft.EntityFrameworkCore;
using BASE.MICRONET.Account.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BASE.MICRONET.Account.Service
{
    public class AccountService : IAccountService
    {
        private readonly ContextDatabase _contextDatabase;

        public AccountService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public bool Deposit(Models.Account account)
        {
            _contextDatabase.Account.Update(account);
            _contextDatabase.SaveChanges();
            return true;
        }

        public IEnumerable<Models.Account> GetAll()
        {
            return _contextDatabase.Account.Include(x => x.Customer)
                .AsNoTracking().ToList();
        }

        public bool Withdrawal(Models.Account account)
        {
            _contextDatabase.Account.Update(account);
            _contextDatabase.SaveChanges();
            return true;
        }
    }
}
