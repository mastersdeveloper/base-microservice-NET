using BASE.MICRONET.History.DTOs;
using BASE.MICRONET.History.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BASE.MICRONET.History.Services
{
    public interface IHistoryService
    {
        Task<IEnumerable<HistoryResponse>> GetAll();

        Task<bool> Add(HistoryTransaction historyTransaction);
    }
}
