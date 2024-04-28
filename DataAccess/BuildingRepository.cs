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
            throw new NotImplementedException();
        }

        public Building CreateBuilding(Building building)
        {
            _context.Set<Building>().Add(building);
            _context.SaveChanges();
            return building;
        }
    }
}
