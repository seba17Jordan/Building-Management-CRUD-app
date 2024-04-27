using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;

namespace BusinessLogic
{
    public class BuildingLogic : IBuildingLogic
    {
        private readonly IBuildingRepository _buildingRepository;
        public BuildingLogic(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }
        public Building CreateBuilding(Building building)
        {
            if (building == null)
            {
                throw new ArgumentNullException(nameof(building), "Building is null");
            }
            if (string.IsNullOrWhiteSpace(building.Name) ||
                string.IsNullOrWhiteSpace(building.Address) ||
                string.IsNullOrWhiteSpace(building.ConstructionCompany))
            {
                throw new ArgumentException("Building invalid data", nameof(building.Name));
            }
            
            return _buildingRepository.CreateBuilding(building);
        }
    }
}
