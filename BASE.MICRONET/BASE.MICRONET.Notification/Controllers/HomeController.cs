using Microsoft.AspNetCore.Mvc;

namespace BASE.MICRONET.Notification.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
