using Microsoft.AspNetCore.Mvc;
using BASE.MICRONET.History.Services;
using System.Linq;
using System.Threading.Tasks;
using BASE.MICRONET.Cross.Cache.Dir;
using System.Collections.Generic;
using BASE.MICRONET.History.DTOs;

namespace BASE.MICRONET.History.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IExtensionCache _extensionCache;

        public HistoryController(IHistoryService historyService, IExtensionCache extensionCache)
        {
            _historyService = historyService;
            _extensionCache = extensionCache;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> Get(int accountId)
        {
            //Manejo de Redis
            string keyHistory = $"keyHistory-{accountId}";
            IEnumerable<HistoryResponse> model = null;

            model = _extensionCache.GetData<IEnumerable<HistoryResponse>>(keyHistory);

            if (model == null)
            {
                var data = await _historyService.GetAll();
                model = data.Where(x => x.AccountId == accountId).ToList();
                _extensionCache.SetData(model, keyHistory, 1);
            }

            return Ok(model);
        }
    }
}
