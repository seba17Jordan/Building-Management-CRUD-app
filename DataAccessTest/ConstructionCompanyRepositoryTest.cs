using DataAccess;
using Domain;
using Domain.@enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessTest
{
    [TestClass]
    public class ConstructionCompanyRepositoryTest
    {
        [TestMethod]
        public void GetConstructionCompanyByName_ShouldReturnConstructionCompanyDataAccessTest()
        {
            // Arrange
            var database = Guid.NewGuid().ToString();
            var context = CreateDbContext(database);
            ConstructionCompany constructionCompany = new ConstructionCompany()
            {
                Name = "Test",
                ConstructionCompanyAdmin = new User()
                {
                    Email = "lkand@gmail.com",
                    Password = "1234",
                    Role = Roles.ConstructionCompanyAdmin,
                    Name = "Lukas",
                    LastName = "Kand"
                }
            };

            context.Add(constructionCompany);
            context.SaveChanges();
            ConstructionCompanyRepository repository = new ConstructionCompanyRepository(context);

            // Act
            ConstructionCompany repoResult = repository.GetConstructionCompanyByName("Test");

            // Assert
            Assert.AreEqual(constructionCompany, repoResult);
        }


        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}
