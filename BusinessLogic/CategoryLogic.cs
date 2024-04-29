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

            if (_categoryRepository.FindCategoryByName(category.Name))
            {
                throw new ArgumentException("Category already exists");
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Invalid data");
            }

            return _categoryRepository.CreateCategory(category);
        }
    }
}