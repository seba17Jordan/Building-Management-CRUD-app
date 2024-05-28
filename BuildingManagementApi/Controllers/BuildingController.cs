
using BuildingManagementApi.Filters;
using Domain;
using Domain.@enum;
using ImportersInterface;
using ImportersLogic;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    [ExceptionFilter]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingLogic _buildingLogic;
        private readonly ISessionService _sessionService;
        private readonly ImporterManager _importerManager;
        //Ruta relativa a la carpeta de los importadores
        private readonly string _importersDirectory = @"..\..\..\..\JsonImporter\Dll\";
        //Ruta relativa a la carpeta de los archivos JSON
        private readonly string _filesDirectory = @"..\..\..\..\JsonImporter\JSONFiles";

        public BuildingController(IBuildingLogic buildingLogic, ISessionService sessionService)
        {
            _buildingLogic = buildingLogic;
            _sessionService = sessionService;
            _importerManager = new ImporterManager();
        }


        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult CreateBuilding([FromBody] BuildingRequest buildingToCreate)
        {
            string token = Request.Headers["Authorization"].ToString();
            var constructionCompanyAdmin = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = buildingToCreate.ToEntity();
            building.ConstructionCompanyAdmin = constructionCompanyAdmin;
            var resultObj = _buildingLogic.CreateBuilding(building, constructionCompanyAdmin);
            BuildingResponse outputResult = new BuildingResponse(resultObj);

            return CreatedAtAction(nameof(CreateBuilding), new { id = outputResult.Id }, outputResult);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult DeleteBuildingById([FromRoute] Guid id)
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));

            _buildingLogic.DeleteBuildingById(id, managerUser.Id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult UpdateBuildingById([FromRoute] Guid id, [FromBody] BuildingRequest buildingUpdates)
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = buildingUpdates.ToEntity();
            var logicBuilding = _buildingLogic.UpdateBuildingById(id, building, managerUser.Id);
            BuildingResponse response = new BuildingResponse(logicBuilding);

            return Ok(response);
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult GetAllBuildings()
        {
            string token = Request.Headers["Authorization"].ToString();
            var ConstructionCompanyUser = _sessionService.GetUserByToken(Guid.Parse(token));

            IEnumerable<BuildingConstructionCompanyResponse> response = _buildingLogic.GetBuildingsByCompanyAdminId(ConstructionCompanyUser.Id).Select(b => new BuildingConstructionCompanyResponse(b)).ToList();
            return Ok(response);
        }

        [HttpPatch("{buildingId}")] //id de building
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult ModifyBuildingManager([FromRoute] Guid buildingId, [FromBody] IdRequest newBuildingManagerId)
        {
            string token = Request.Headers["Authorization"].ToString();
            var constructionCompanyAdmin = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = _buildingLogic.ModifyBuildingManager(buildingId, newBuildingManagerId.Id, constructionCompanyAdmin.Id);
            BuildingResponse response = new BuildingResponse(building);

            return Ok(response);
        }

        [HttpPost("import")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult ImportBuildings([FromBody] ImportRequest importRequest) //importer name, JSON file Name
        {
            var importer = _importerManager.GetImporter(_importersDirectory, importRequest.ImporterName);
            if (importer == null)
            {
                return BadRequest("Invalid importer specified.");
            }

            string filePath = Path.Combine(_filesDirectory, importRequest.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("File not found.");
            }

            importer.Import(filePath);
            
            return Ok();
        }
    }
}
