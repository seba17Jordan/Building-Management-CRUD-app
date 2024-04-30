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
        
        public bool FindCategoryById(Guid id)
        {
            return _context.Set<Category>().Any(c => c.Id == id);
        }

        public bool FindCategoryByName(string name)
        {
            return _context.Set<Category>().Any(c => c.Name == name);
        }

        public Category GetCategoryById(Guid category)
        {
            return _context.Set<Category>().Find(category);
        }
    }
}
