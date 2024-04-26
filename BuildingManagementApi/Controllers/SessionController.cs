using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;

namespace BuildingManagementApi.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var token = _sessionService.Authenticate(userLoginRequest.Email, userLoginRequest.Password);
            return Ok(token);
        }
    }
}
