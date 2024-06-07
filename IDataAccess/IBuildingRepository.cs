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
        IEnumerable<Building> GetAllBuildingsByManager(Guid managerId);
        Apartment GetApartmentById(Guid apartment);
        Building GetBuildingById(Guid id);
        Building GetBuildingByName(string buildingName);
        Guid GetBuildingIdByApartment(Apartment apartment);
        void Save();
        void UpdateBuilding(Building buildingToUpdate);
    }
}
