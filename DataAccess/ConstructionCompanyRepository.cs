using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ConstructionCompanyRepository : IConstructionCompanyRepository
    {
        private readonly DbContext _context;
        public ConstructionCompanyRepository(DbContext context)
        {
            _context = context;
        }
        public ConstructionCompany GetConstructionCompanyByName(string name)
        {
            return _context.Set<ConstructionCompany>().FirstOrDefault(x => x.Name == name);
        }
    }
}
