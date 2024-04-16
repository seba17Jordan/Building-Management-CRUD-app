using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorLogic _administratorLogic;

        public AdministratorController(IAdministratorLogic administratorLogic)
        {
            this._administratorLogic = administratorLogic;
        }

        [HttpPost]
        public IActionResult CreateAdministrator([FromBody] AdministratorRequest adminToCreate)
        {
            var admin = adminToCreate.ToEntity(); // Convierte AdministratorRequest a Administrator
            var resultAdmin = _administratorLogic.CreateAdministrator(admin); // Llama al método de lógica para crear el administrador
            var response = new AdministratorResponse(resultAdmin); // Convierte Administrator a AdministratorResponse

            return CreatedAtAction(nameof(CreateAdministrator), new { id = response.Id }, response);
        }
    }
}

