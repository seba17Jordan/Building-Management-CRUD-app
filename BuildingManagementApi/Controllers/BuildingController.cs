
using BuildingManagementApi.Filters;
using Domain.@enum;
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

        public BuildingController(IBuildingLogic buildingLogic, ISessionService sessionService)
        {
            this._buildingLogic = buildingLogic;
            _sessionService = sessionService;
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
            var resultObj = _buildingLogic.CreateBuilding(building);
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

        [HttpGet("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult GetBuildingById(Guid id)
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = _buildingLogic.GetBuildingById(id);

            if (building.ConstructionCompanyAdmin.Id != managerUser.Id)
            {
                return Unauthorized();
            }

            var hasManager = building.ConstructionCompanyAdmin != null;
            var managerName = hasManager ? $"{building.ConstructionCompanyAdmin.Name} {building.ConstructionCompanyAdmin.LastName}" : null;

            var listBuildingResponse = new ListBuildingResponse(
                building.Id,
                building.Name,
                building.Address,
                hasManager,
                managerName
            );

            return Ok(listBuildingResponse);
        }

    }
}
