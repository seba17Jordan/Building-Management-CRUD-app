
using BuildingManagementApi.Filters;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingLogic _buildingLogic;

        public BuildingController(IBuildingLogic buildingLogic)
        {
            this._buildingLogic = buildingLogic;
        }


        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult CreateBuilding([FromBody] BuildingRequest buildingToCreate)
        {
            var building = buildingToCreate.ToEntity();
            var resultObj = _buildingLogic.CreateBuilding(building);
            BuildingResponse outputResult = new BuildingResponse(resultObj);

            return CreatedAtAction(nameof(CreateBuilding), new { id = outputResult.Id }, outputResult);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult DeleteBuildingById(Guid id)
        {
            _buildingLogic.DeleteBuildingById(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult UpdateBuildingById(Guid id, [FromBody] BuildingRequest buildingUpdates)
        {
            var building = buildingUpdates.ToEntity();
            var logicBuilding = _buildingLogic.UpdateBuildingById(id, building);
            BuildingResponse response = new BuildingResponse(logicBuilding);

            return Ok(response);
        }

    }
}
