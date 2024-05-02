using BuildingManagementApi.Filters;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;

namespace BuildingManagementApi.Controllers
{
    [Route("api/session")]
    [ApiController]
    [ExceptionFilter]
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

        [HttpDelete]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult Logout()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            Guid token = Guid.Parse(authHeader);
            _sessionService.Logout(token);
            return Ok("Logout success");
        }
    }
}
