
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;

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
        public IActionResult CreateBuilding([FromBody] BuildingRequest buildingToCreate)
        {
            throw new NotImplementedException();
        }
    }
}
