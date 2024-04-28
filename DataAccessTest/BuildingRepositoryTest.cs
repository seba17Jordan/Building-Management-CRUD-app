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

        [TestMethod]
        public void BuildingExistsTestDataAccess()
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

            var context = CreateDbContext("BuildingExistsTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.SaveChanges();

            // Assert
            Assert.IsTrue(buildingRepo.BuildingNameExists(createdBuilding.Name));
        }

        [TestMethod]
        public void GetBuildingByIdTestDataAccess()
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
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = ""}
                    }
                }
            };
            var context = CreateDbContext("GetBuildingByIdTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(createdBuilding, buildingRepo.GetBuildingById(createdBuilding.Id));
        }

        [TestMethod]
        public void DeleteBuildingTestDataAccess()
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
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = ""}
                    }
                }
            };
            var context = CreateDbContext("DeleteBuildingTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.SaveChanges();
            buildingRepo.DeleteBuilding(createdBuilding);
            context.SaveChanges();

            // Assert
            Assert.IsNull(buildingRepo.GetBuildingById(createdBuilding.Id));
        }


        [TestMethod]
        public void UpdateBuildingTestDataAccess()
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
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = ""}
                    }
                }
            };
            var context = CreateDbContext("UpdateBuildingTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.SaveChanges();

            Building updatedBuilding = new Building()
            {
                Id = createdBuilding.Id,
                Name = "Building 2",
                Address = "Address 2",
                ConstructionCompany = "Construction Company 2",
                CommonExpenses = 200,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 2,
                        Number = 201,
                        Owner = new Owner { Name = "John", LastName = "Doe", Email = ""}
                    }
                }
            };
            context.Entry(createdBuilding).State = EntityState.Detached;
            buildingRepo.UpdateBuilding(updatedBuilding);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(updatedBuilding, buildingRepo.GetBuildingById(createdBuilding.Id));
        }


        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}