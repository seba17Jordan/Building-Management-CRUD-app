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
    [Route("api/[controller]")]
    [TypeFilter(typeof(ExceptionFilter))] 
    [TypeFilter(typeof(AuthenticationFilter))]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorLogic _administratorLogic;

        public AdministratorController(IAdministratorLogic administratorLogic)
        {
            this._administratorLogic = administratorLogic;
        }

        [HttpPost]
        [AuthenticationFilter([Roles.Administrator])]
        public IActionResult CreateAdministrator([FromBody] AdministratorRequest adminToCreate)
        {
            var admin = adminToCreate.ToEntity();
            var resultAdmin = _administratorLogic.CreateAdministrator(admin);
            var response = new AdministratorResponse(resultAdmin);

            return CreatedAtAction(nameof(CreateAdministrator), new { id = response.Id }, response);
        }
    }
}

