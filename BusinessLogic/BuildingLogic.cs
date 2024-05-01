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

            building.SelfValidate();

            foreach (var apartment in building.Apartments)
            {
                apartment.SelfValidate();
                apartment.Owner.SelfValidate();
            }
            
            if (_buildingRepository.BuildingNameExists(building.Name))
            {
                throw new ArgumentException("Building with same name already exists");
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
                        apartment.Owner = null;
                    }
                    _buildingRepository.DeleteApartment(apartment);
                }
                building.Apartments.Clear();
                building.Apartments = null;
            }
            _buildingRepository.Save();
            _buildingRepository.DeleteBuilding(building);
            _buildingRepository.Save();
        }

        public Building UpdateBuildingById(Guid id, Building building)
        {
            Building buildingToUpdate = _buildingRepository.GetBuildingById(id);
            
            if (_buildingRepository.BuildingNameExists(building.Name)) //ver si name es distinto de null
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
                buildingToUpdate.Apartments.Clear();
                foreach (var apartment in building.Apartments)
                {
                    var conedApartment = new Apartment
                    {
                        Number = apartment.Number,
                        Floor = apartment.Floor,
                        Owner = apartment.Owner,
                        Rooms = apartment.Rooms,
                        Bathrooms = apartment.Bathrooms,
                        HasTerrace = apartment.HasTerrace,
                    };
                    buildingToUpdate.Apartments.Add(conedApartment);
                }
            }

            _buildingRepository.UpdateBuilding(buildingToUpdate);
            _buildingRepository.Save();

            return buildingToUpdate;
        }
    }
}
