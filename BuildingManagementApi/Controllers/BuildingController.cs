
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
    [TypeFilter(typeof(ExceptionFilter))]
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
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult CreateBuilding([FromBody] BuildingRequest buildingToCreate)
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = buildingToCreate.ToEntity();
            building.managerId = managerUser.Id;
            var resultObj = _buildingLogic.CreateBuilding(building);
            BuildingResponse outputResult = new BuildingResponse(resultObj);

            return CreatedAtAction(nameof(CreateBuilding), new { id = outputResult.Id }, outputResult);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult DeleteBuildingById([FromRoute] Guid id)
        {
            _buildingLogic.DeleteBuildingById(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult UpdateBuildingById([FromRoute] Guid id, [FromBody] BuildingRequest buildingUpdates)
        {
            var building = buildingUpdates.ToEntity();
            var logicBuilding = _buildingLogic.UpdateBuildingById(id, building);
            BuildingResponse response = new BuildingResponse(logicBuilding);

            return Ok(response);
        }

    }
}
