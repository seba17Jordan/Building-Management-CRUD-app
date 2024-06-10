using CustomExceptions;
using Domain;
using IDataAccess;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CategoryLogic : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryLogic(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Category CreateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category can't be null");
            }

            category.SelfValidate();

            if (_categoryRepository.FindCategoryByName(category.Name))
            {
                throw new ObjectAlreadyExistsException("Category already exists");
            }

            return _categoryRepository.CreateCategory(category);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }
    }
}