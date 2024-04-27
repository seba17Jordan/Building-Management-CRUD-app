using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbContext _context;

        public CategoryRepository(DbContext context)
        {
            _context = context;
        }

        public Category CreateCategory(Category category)
        {
            _context.Set<Category>().Add(category);
            _context.SaveChanges();
            return category;
        }

        public bool CategoryExists(string name)
        {
            return _context.Set<Category>().Any(c => c.Name == name);
        }
    }
}
