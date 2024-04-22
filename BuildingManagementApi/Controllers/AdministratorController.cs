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
        [AuthenticationFilter(new Roles[] { Roles.Administrator })]
        public IActionResult CreateAdministrator([FromBody] AdministratorRequest adminToCreate)
        {
            var admin = adminToCreate.ToEntity(); // Convierte AdministratorRequest a Administrator
            var resultAdmin = _administratorLogic.CreateAdministrator(admin); // Llama al método de lógica para crear el administrador
            var response = new AdministratorResponse(resultAdmin); // Convierte Administrator a AdministratorResponse

            return CreatedAtAction(nameof(CreateAdministrator), new { id = response.Id }, response);
        }
    }
}

