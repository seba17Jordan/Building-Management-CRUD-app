using ImportersInterface;
using LogicInterface;
using ModelsApi.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportersLogic
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingLogic _buildingLogic;
        private readonly ISessionService _sessionService;

        public BuildingService(IBuildingLogic buildingLogic, ISessionService sessionService)
        {
            _buildingLogic = buildingLogic;
            _sessionService = sessionService;
        }

        public void CreateBuilding(BuildingRequest buildingRequest)
        {
            string token = "e4fe348e-0fe3-4f1b-80a7-347dee4225d1";
            var constructionCompanyAdmin = _sessionService.GetUserByToken(Guid.Parse(token));

            var building = buildingRequest.ToEntity();
            building.ConstructionCompanyAdmin = constructionCompanyAdmin;
            _buildingLogic.CreateBuilding(building, constructionCompanyAdmin);
        }
    }
}
