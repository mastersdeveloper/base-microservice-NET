using BASE.MICRONET.Security.Services;
using Microsoft.AspNetCore.Mvc;

namespace BASE.MICRONET.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccessService _accessService;

        public AuthController(IAccessService accessService)
        {
            _accessService = accessService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_accessService.GetAll());
        }
    }
}
