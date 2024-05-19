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
        Apartment GetApartmentById(Guid apartment);
        Building GetBuildingById(Guid id);
        Building GetBuildingByName(string buildingName);
        Guid GetBuildingIdByApartmentId(Apartment apartment);
        ConstructionCompany GetConstructionCompanyByName(string name);
        void Save();
        void UpdateBuilding(Building buildingToUpdate);
    }
}
