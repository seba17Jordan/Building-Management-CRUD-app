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

        public ConstructionCompany CreateConstructionCompany(ConstructionCompany constructionCompany)
        {
            _context.Set<ConstructionCompany>().Add(constructionCompany);
            _context.SaveChanges();
            return constructionCompany;
        }

        public ConstructionCompany GetConstructionCompanyByAdmin(Guid id)
        {
            return _context.Set<ConstructionCompany>().FirstOrDefault(x => x.ConstructionCompanyAdmin.Id == id);
        }

        public ConstructionCompany UpdateConstructionCompany(ConstructionCompany companyToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
