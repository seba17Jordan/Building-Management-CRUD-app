using Domain;

namespace IDataAccess
{
    public interface IBuildingRepository
    {
        bool BuildingNameExists(string name);
        Building CreateBuilding(Building building);
    }
}
