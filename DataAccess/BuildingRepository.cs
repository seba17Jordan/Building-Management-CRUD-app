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

        //nuevo
        public void DeleteApartment(Apartment apartment)
        {
            _context.Set<Apartment>().Remove(apartment);
        }

        public void DeleteBuilding(Building building)
        {
            _context.Set<Building>().Remove(building);
        }

        public Building GetBuildingById(Guid id)
        {
            return _context.Set<Building>()
                .Include(m => m.Manager)
                .Include(b => b.Apartments)
                .ThenInclude(o => o.Owner)
                .FirstOrDefault(b => b.Id == id);
        }

        public bool ExistApartment(Guid id)
        {
            return _context.Set<Apartment>().Any(a => a.Id == id);
        }

        public List<Building> GetAllBuildings(Guid constructionCompanyAdminId) { 
            return _context.Set<Building>().Where(b => b.ConstructionCompanyAdmin.Id == constructionCompanyAdminId).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateBuilding(Building buildingToUpdate)
        {
            _context.Set<Building>().Update(buildingToUpdate);
        }

        public Guid GetBuildingIdByApartmentId(Apartment apartment)
        {
            return _context.Set<Building>().FirstOrDefault(b => b.Apartments.Contains(apartment)).Id;
        }

        public Apartment GetApartmentById(Guid apartment)
        {
            return _context.Set<Apartment>().FirstOrDefault(a => a.Id == apartment);
        }

        public Building GetBuildingByName(string buildingName)
        {
            return _context.Set<Building>().FirstOrDefault(b => b.Name == buildingName);
        }
    }
}
