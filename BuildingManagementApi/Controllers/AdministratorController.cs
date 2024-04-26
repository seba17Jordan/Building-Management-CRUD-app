using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelsApi;
using ModelsApi.In;
using ModelsApi.Out;
using BuildingManagementApi.Filters;
using Domain.@enum;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/admins")]
    //[TypeFilter(typeof(ExceptionFilter))] 
    public class AdministratorController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public AdministratorController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        //[AuthenticationFilter([Roles.Administrator])]
        public IActionResult CreateAdministrator([FromBody] AdministratorRequest adminToCreate)
        {
            var admin = adminToCreate.ToEntity();
            var resultAdmin = _userLogic.CreateUser(admin);
            var response = new AdministratorResponse(resultAdmin);

            return CreatedAtAction(nameof(CreateAdministrator), new { id = response.Id }, response);
        }
    }
}

