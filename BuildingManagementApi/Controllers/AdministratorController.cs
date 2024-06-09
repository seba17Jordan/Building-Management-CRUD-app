using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelsApi;
using ModelsApi.In;
using ModelsApi.Out;
using BuildingManagementApi.Filters;
using Domain.@enum;
using BusinessLogic;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/admins")]
    [ExceptionFilter]
    public class AdministratorController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public AdministratorController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        //[ServiceFilter(typeof(AuthenticationFilter))]
        //[AuthorizationFilter(_currentRole = Roles.Administrator)]
        public IActionResult CreateAdministrator([FromBody] AdministratorRequest adminToCreate)
        {
            var admin = adminToCreate.ToEntity();
            var resultAdmin = _userLogic.CreateUser(admin);
            AdministratorResponse response = new AdministratorResponse(resultAdmin);

            return CreatedAtAction(nameof(CreateAdministrator), new { id = response.Id }, response);
        }

        [HttpGet("managers")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult GetAllManagers()
        {
            IEnumerable<UserResponse> response = _userLogic.GetAllManagers().Select(m => new UserResponse(m)).ToList();
            return Ok(response);
        }

        [HttpGet("maintenance")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult GetAllMaintenancePersons()
        {
            IEnumerable<UserResponse> response = _userLogic.GetAllMaintenancePersons().Select(m => new UserResponse(m)).ToList();
            return Ok(response);
        }
    }
}

