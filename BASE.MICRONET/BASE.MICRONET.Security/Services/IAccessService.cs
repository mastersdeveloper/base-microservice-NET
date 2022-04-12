using BASE.MICRONET.Security.Models;
using System.Collections.Generic;

namespace BASE.MICRONET.Security.Services
{
    public interface IAccessService
    {
        IEnumerable<Access> GetAll();
        bool Validate(string userName, string password);
    }
}
