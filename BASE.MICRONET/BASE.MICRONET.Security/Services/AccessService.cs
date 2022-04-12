using BASE.MICRONET.Security.Models;
using BASE.MICRONET.Security.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BASE.MICRONET.Security.Services
{
    public class AccessService : IAccessService
    {
        private readonly ContextDatabase _contextDatabase;

        public AccessService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public IEnumerable<Access> GetAll()
        {
            return _contextDatabase.Access.ToList();
        }

        public bool Validate(string userName, string password)
        {
            var list = _contextDatabase.Access.ToList();

            var access = list.Where(x => x.Username == userName && x.Password == password)
                .FirstOrDefault();

            if (access != null)
                return true;

            return false;
        }
    }
}
