using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly DbContext _context;

        public BuildingRepository(DbContext context)
        {
            _context = context;
        }

        public bool BuildingNameExists(string name)
        {
            return _context.Set<Building>().Any(b => b.Name == name);
        }

        public Building CreateBuilding(Building building)
        {
            _context.Set<Building>().Add(building);
            _context.SaveChanges();
            return building;
        }

        public void DeleteBuilding(Building building)
        {
            _context.Set<Building>().Remove(building);
        }

        public Building GetBuildingById(Guid id)
        {
            return _context.Set<Building>().Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateBuilding(Building buildingToUpdate)
        {
            _context.Set<Building>().Update(buildingToUpdate);
            _context.SaveChanges();
        }
    }
}
