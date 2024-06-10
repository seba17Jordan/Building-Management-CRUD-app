using DataAccess;
using Domain;
using Domain.@enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessTest
{
    [TestClass]
    public class CategoryRepositoryTest
    {
        [TestMethod]
        public void CreateCategoryCorrectTest()
        {
            // Arrange
            var expectedCategory = new Category
            {
                Name = "category"
            };

            var context = CreateDbContext("CreateCategoryCorrectTest");
            var categoryRepository = new CategoryRepository(context);

            // Act
            Category createdCategory = categoryRepository.CreateCategory(expectedCategory);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedCategory, createdCategory);
        }

        [TestMethod]
        public void CategoryNameExistsTest()
        {
            // Arrange
            var expectedCategory = new Category
            {
                Name = "category"
            };

            var context = CreateDbContext("CreateCategoryExistsTest");
            var categoryRepository = new CategoryRepository(context);

            // Act
            Category createdCategory = categoryRepository.CreateCategory(expectedCategory);
            context.SaveChanges();

            // Assert
            Assert.IsTrue(categoryRepository.FindCategoryByName(expectedCategory.Name));
        }

        [TestMethod]
        public void FindCategoryByIdCorrectTest()
        {
            // Arrange
            var expectedCategory = new Category
            {
                Name = "category"
            };

            var context = CreateDbContext("FindCategoryByIdCorrectTest");
            var categoryRepository = new CategoryRepository(context);

            // Act
            Category createdCategory = categoryRepository.CreateCategory(expectedCategory);
            context.SaveChanges();

            // Assert
            Assert.IsTrue(categoryRepository.FindCategoryById(createdCategory.Id));
        }

        [TestMethod]
        public void GetCategoryByIdCorrectTest()
        {
            // Arrange
            var expectedCategory = new Category
            {
                Name = "category"
            };

            var context = CreateDbContext("GetCategoryByIdCorrectTest");
            var categoryRepository = new CategoryRepository(context);

            // Act
            Category createdCategory = categoryRepository.CreateCategory(expectedCategory);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedCategory, categoryRepository.GetCategoryById(createdCategory.Id));
        }

        [TestMethod]
        public void GetAllCategories_ReturnsCategories_Successfully()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category1" },
                new Category { Id = Guid.NewGuid(), Name = "Category2" },
                new Category { Id = Guid.NewGuid(), Name = "Category3" }
            };

            var dbContextOptions = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "GetAllCategories_ReturnsCategories_Successfully")
                .Options;

            using (var context = new Context(dbContextOptions))
            {
                context.Categories.AddRange(expectedCategories);
                context.SaveChanges();

                var categoryRepository = new CategoryRepository(context);

                // Act
                var result = categoryRepository.GetAllCategories();

                // Assert
                Assert.IsNotNull(result);
                CollectionAssert.AreEquivalent(expectedCategories, result.ToList());
            }
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}