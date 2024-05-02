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
    [Route("api/maintenances")]
    [ExceptionFilter]
    public class MaintenancePersonController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public MaintenancePersonController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult CreateMaintenancePerson([FromBody] MaintenancePersonRequest maintenancePersonToCreate)
        {
            var maintenancePerson = maintenancePersonToCreate.ToEntity();
            var resultMaintenance = _userLogic.CreateUser(maintenancePerson);
            MaintenancePersonResponse response = new MaintenancePersonResponse(resultMaintenance);

            return CreatedAtAction(nameof(CreateMaintenancePerson), new { id = response.Id }, response);
        }
    }
}

