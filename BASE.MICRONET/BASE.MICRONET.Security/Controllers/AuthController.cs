using BASE.MICRONET.Cross.Token.Dir;
using BASE.MICRONET.Security.DTOs;
using BASE.MICRONET.Security.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BASE.MICRONET.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccessService _accessService;
        private readonly ILogger<AuthController> _log;
        private readonly JwtOptions _jwtOption;

        public AuthController(IAccessService accessService,
            IOptionsSnapshot<JwtOptions> jwtOption, ILogger<AuthController> log)
        {
            _accessService = accessService;
            _log = log;
            _jwtOption = jwtOption.Value;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_accessService.GetAll());
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest request)
        {
            _log.LogInformation("Start Post in AuthController");
            var app = "MyApp";
            _log.LogInformation($"App: {app}");

            if (!_accessService.Validate(request.UserName, request.Password))
            {
                return Unauthorized();
            }

            Response.Headers.Add("access-control-expose-headers", "Authorization");
            Response.Headers.Add("Authorization", JwtToken.Create(_jwtOption));

            return Ok();
        }
    }
}
