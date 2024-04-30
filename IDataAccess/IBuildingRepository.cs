using Domain;

namespace IDataAccess
{
    public interface IBuildingRepository
    {
        bool BuildingNameExists(string name);
        Building CreateBuilding(Building building);
        void DeleteApartment(Apartment apartment);
        void DeleteBuilding(Building building);
        bool ExistApartment(Guid apartment);
        List<Building> GetAllBuildings(Guid userId);
        Building GetBuildingById(Guid id);
        void Save();
        void UpdateBuilding(Building buildingToUpdate);
    }
}
