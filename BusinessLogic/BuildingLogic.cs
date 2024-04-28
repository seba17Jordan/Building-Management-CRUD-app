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
            
            if (_buildingRepository.BuildingNameExists(building.Name))
            {
                throw new ArgumentException("Building with same name already exists");
            }

            if (building.CommonExpenses < 0)
            {
                throw new ArgumentException("Common expenses must be greater than 0", nameof(building.CommonExpenses));
            }
            if (building.Apartments.Count == 0 || building.Apartments == null)
            {
                throw new ArgumentException("Building must have at least one apartment", nameof(building.Apartments));
            }
            return _buildingRepository.CreateBuilding(building);
        }

        public void DeleteBuilding(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
