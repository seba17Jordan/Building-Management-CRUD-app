
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
        private readonly IBuildingService _buildingService;
        private readonly ImporterManager _importerManager;
        //Ruta relativa a la carpeta de los importadores
        private readonly string _importersDirectory = @"..\JsonImporter\bin\Debug\net8.0";
        //Ruta relativa a la carpeta de los archivos JSON
        private readonly string _filesDirectory = @"..\JsonImporter\JSONFiles";

        public BuildingController(IBuildingLogic buildingLogic, ISessionService sessionService, IBuildingService buildingService)
        {
            _buildingLogic = buildingLogic;
            _sessionService = sessionService;
            _buildingService = buildingService;
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
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult DeleteBuildingById([FromRoute] Guid id)
        {
            string token = Request.Headers["Authorization"].ToString();
            var companyAdmin = _sessionService.GetUserByToken(Guid.Parse(token));

            _buildingLogic.DeleteBuildingById(id, companyAdmin.Id);
            return NoContent();
        }

        [HttpPatch("detail/{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult UpdateBuildingById([FromRoute] Guid id, [FromBody] BuildingRequest buildingUpdates)
        {
            string token = Request.Headers["Authorization"].ToString();
            var constructionCompanyAdmin = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = buildingUpdates.ToEntity();
            var logicBuilding = _buildingLogic.UpdateBuildingById(id, building, constructionCompanyAdmin.Id);
            BuildingResponse response = new BuildingResponse(logicBuilding);

            return Ok(response);
        }

        [HttpGet("company-admin")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult GetAllBuildingsCompanyAdmin()
        {
            string token = Request.Headers["Authorization"].ToString();
            var ConstructionCompanyUser = _sessionService.GetUserByToken(Guid.Parse(token));

            IEnumerable<BuildingResponse> response = _buildingLogic.GetBuildingsByCompanyAdminId(ConstructionCompanyUser.Id).Select(b => new BuildingResponse(b)).ToList();
            return Ok(response);
        }

        [HttpGet("manager")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult GetAllBuildingsByManager()
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));
            IEnumerable<BuildingResponse> response = _buildingLogic.GetBuildingsByManagerId(managerUser.Id).Select(b => new BuildingResponse(b)).ToList();

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
            string token = Request.Headers["Authorization"].ToString();
            Console.WriteLine("Importador: "+ importRequest.ImporterName);
            Console.WriteLine("Archivo: "+ importRequest.FileName);

            var importer = ImporterManager.GetImporter(_importersDirectory, importRequest.ImporterName);
            if (importer == null)
            {
                return BadRequest("Invalid importer specified.");
            }
            
            string filePath = Path.Combine(_filesDirectory, importRequest.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("File not found.");
            }

            importer.Import(filePath, _buildingService, token);

            //retorno ok con un mensaje dentro
            return Ok(new { message = "Import success" });
        }

        [HttpGet("importers")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult GetImporters()
        {
            var importers = ImporterManager.GetAvailableImporters(_importersDirectory);
            return Ok(importers);
        }

        [HttpGet("files")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult GetFiles()
        {
            var files = Directory.GetFiles(_filesDirectory).Select(Path.GetFileName).ToList();
            return Ok(files);
        }

        [HttpGet]
        public IActionResult GetAllBuildings()
        {
            var buildings = _buildingLogic.GetAllBuildings();
            IEnumerable<BuildingResponse> response = buildings.Select(b => new BuildingResponse(b)).ToList();
            return Ok(response);
        }

        [HttpGet("manager/apartments")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult GetAllApartmentsByManager()
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));
            
            var buildings = _buildingLogic.GetBuildingsByManagerId(managerUser.Id);


            if (buildings != null && buildings.Any())
            {
                // Obtener los apartamentos de todos los edificios
                var apartmentsWithBuilding = buildings.SelectMany(b => b.Apartments, (building, apartment) => new ApartmentWithBuildingNameResponse(apartment, building.Name)).ToList();
                
                return Ok(apartmentsWithBuilding);
            }
            else
            {
                return Ok(new List<ApartmentResponse>()); 
            }
        }
    }
}
