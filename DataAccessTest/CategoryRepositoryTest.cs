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

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}