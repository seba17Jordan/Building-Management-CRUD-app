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

        [TestMethod]
        public void CreateConstructionCompany_ShouldReturnConstructionCompanyDataAccessTest()
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

            ConstructionCompanyRepository repository = new ConstructionCompanyRepository(context);

            // Act
            ConstructionCompany repoResult = repository.CreateConstructionCompany(constructionCompany);

            // Assert
            Assert.AreEqual(constructionCompany, repoResult);
        }

        [TestMethod]
        public void UpdateConstructionCompanyDataAccessTest()
        {
            // Arrange
            User constructionCompanyAdmin = new User()
            {
                Email = "s@gmial.com",
                Password = "1234",
                Role = Roles.ConstructionCompanyAdmin,
                Name = "Sara",
                LastName = "Kand"
            };

            ConstructionCompany expectedConstructionCompany = new ConstructionCompany()
            {
                Name = "Test",
                ConstructionCompanyAdmin = constructionCompanyAdmin
            };

            var context = CreateDbContext("UpdateConstructionCompanyDataAccessTest");
            ConstructionCompanyRepository repository = new ConstructionCompanyRepository(context);

            // Act
            ConstructionCompany constructionCompanyCreated = repository.CreateConstructionCompany(expectedConstructionCompany);
            expectedConstructionCompany.Name = "Test2";
            ConstructionCompany updatedCompany = repository.UpdateConstructionCompany(constructionCompanyCreated);

            // Assert
            Assert.AreEqual(repository.GetConstructionCompanyByAdmin(constructionCompanyAdmin.Id).Name, "Test2");
        }
        


        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}
