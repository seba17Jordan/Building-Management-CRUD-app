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
            throw new NotImplementedException();
        }

        public Building GetBuildingById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
