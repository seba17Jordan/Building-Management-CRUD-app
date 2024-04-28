using DataAccess;
using Domain;
using Domain.@enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessTest
{
    [TestClass]
    public class BuildingRepositoryTest
    {
        [TestMethod]
        public void CreateBuildingCorrectTestDataAccess()
        {
            // Arrange
            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = "Construction Company 1",
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                }
            };

            var context = CreateDbContext("CreateBuildingCorrectTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedBuilding, createdBuilding);
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}