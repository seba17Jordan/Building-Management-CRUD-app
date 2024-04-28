using Domain;

namespace IDataAccess
{
    public interface IBuildingRepository
    {
        bool BuildingNameExists(string name);
        Building CreateBuilding(Building building);
        void DeleteApartment(Apartment apartment);
        void DeleteBuilding(Building building);
        void DeleteOwner(Owner owner);
        Building GetBuildingById(Guid id);
        void Save();
        void UpdateBuilding(Building buildingToUpdate);
    }
}
