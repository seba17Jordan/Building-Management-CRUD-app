using Domain;

namespace IDataAccess
{
    public interface IBuildingRepository
    {
        bool BuildingNameExists(string name);
        Building CreateBuilding(Building building);
        void DeleteBuilding(Building building);
        Building GetBuildingById(Guid id);
        void Save();
    }
}
