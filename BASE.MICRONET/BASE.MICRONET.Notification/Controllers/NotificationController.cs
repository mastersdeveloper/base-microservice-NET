using BASE.MICRONET.Notification.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BASE.MICRONET.Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _notificationService.GetAll();
            var model = result.ToList();
            return Ok(model);
        }
    }
}
