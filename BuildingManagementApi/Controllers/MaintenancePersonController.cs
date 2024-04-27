using BuildingManagementApi.Filters;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class MaintenancePersonController : ControllerBase
    {
        private readonly IMaintenancePersonLogic _maintenancePersonLogic;

        public MaintenancePersonController(IMaintenancePersonLogic maintenancePersonLogic)
        {
            _maintenancePersonLogic = maintenancePersonLogic;
        }

        [HttpPost]
        //[AuthenticationFilter([Roles.Manager])]
        public IActionResult CreateMaintenancePerson([FromBody] MaintenancePersonRequest maintenancePersonRequest)
        {
            var maintenancePerson = maintenancePersonRequest.ToEntity();
            var createdMaintenancePerson = _maintenancePersonLogic.CreateMaintenancePerson(maintenancePerson);
            var outputResult = new MaintenancePersonResponse(createdMaintenancePerson);

            return CreatedAtAction(nameof(CreateMaintenancePerson), new { id = outputResult.Id }, outputResult);
        }

    }
}
