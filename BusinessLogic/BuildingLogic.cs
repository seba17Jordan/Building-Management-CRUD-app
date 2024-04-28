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

        public void DeleteBuildingById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id is empty", nameof(id));
            }
            Building building = _buildingRepository.GetBuildingById(id);
            if (building == null)
            {
                throw new ArgumentException("Building not found", nameof(id));
            }
            
            if (building.Apartments != null)
            {
                foreach (var apartment in building.Apartments)
                {
                    if (apartment.Owner != null)
                    {
                        _buildingRepository.DeleteOwner(apartment.Owner);
                    }
                    _buildingRepository.DeleteApartment(apartment);
                }
                building.Apartments.Clear();
                building.Apartments = null;
            }

            _buildingRepository.DeleteBuilding(building);
            _buildingRepository.Save();
        }

        public Building UpdateBuildingById(Guid id, Building building)
        {
            Building buildingToUpdate = _buildingRepository.GetBuildingById(id);
            
            if (_buildingRepository.BuildingNameExists(building.Name))
            {
                throw new ArgumentException("Building with same name already exists");
            }

            if (buildingToUpdate == null)
            {
                throw new ArgumentException("Building not found", nameof(id));
            }

            if (building.Name != null)
            {
                buildingToUpdate.Name = building.Name;
            }

            if (building.Address != null)
            {
                buildingToUpdate.Address = building.Address;
            }

            if (building.ConstructionCompany != null)
            {
                buildingToUpdate.ConstructionCompany = building.ConstructionCompany;
            }

            if (building.CommonExpenses != null)
            {
                buildingToUpdate.CommonExpenses = building.CommonExpenses;
            }

            if (building.Apartments != null && building.Apartments.Count >=1)
            {
                buildingToUpdate.Apartments = building.Apartments;
            }

            _buildingRepository.UpdateBuilding(buildingToUpdate);
            _buildingRepository.Save();

            return buildingToUpdate;
        }
    }
}
