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
                ConstructionCompany = new ConstructionCompany("Construction Company"),
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
                ConstructionCompany = new ConstructionCompany("Construction Company"),
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

        /*
        [TestMethod]
        public void GetBuildingByIdTestDataAccess()
        {
            User comanyAdmin = new User
            {
                Name = "Company Admin",
                LastName = "Company Admin",
                Email = ""
            };

            ConstructionCompany constructionCompany = new ConstructionCompany("Construction Company");
            constructionCompany.ConstructionCompanyAdmin = comanyAdmin;

            // Arrange
            Building expectedBuilding = new Building()
            {
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = constructionCompany,
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = ""}
                    }
                },
                ConstructionCompanyAdmin = comanyAdmin,
            };
            var context = CreateDbContext("GetBuildingByIdTestDataAccess");
            BuildingRepository buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.Entry(createdBuilding).State = EntityState.Detached;
            Building building = buildingRepo.GetBuildingById(createdBuilding.Id);
            // Assert
            Assert.AreEqual(createdBuilding, building);
        }*/

        [TestMethod]
        public void DeleteBuildingTestDataAccess()
        {
            // Arrange
            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
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
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
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
            BuildingRepository buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(expectedBuilding);
            context.Entry(createdBuilding).State = EntityState.Detached; // Para asegurarse de que no haya seguimiento
            createdBuilding.Name = "Building 2";

            buildingRepo.UpdateBuilding(createdBuilding);
            context.SaveChanges();

            // Assert
            Assert.AreEqual("Building 2", createdBuilding.Name);
        }


        [TestMethod]
        public void DeleteApartmentFromBuildingTestDataAccess()
        {
            // Arrange
            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
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
            var context = CreateDbContext("DeleteApartmentFromBuildingTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            Building createdBuilding = buildingRepo.CreateBuilding(building);
            context.SaveChanges();
            buildingRepo.DeleteApartment(createdBuilding.Apartments.First());
            context.SaveChanges();

            // Assert
            Assert.AreEqual(0, createdBuilding.Apartments.Count);
        }

        [TestMethod]
        public void ExistApartmentFromBuildingTestDataAccess()
        {
            // Arrange
            Apartment apartment = new Apartment()
            {
                Id = Guid.NewGuid(),
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = ""}
            };
            var context = CreateDbContext("ExistApartmentFromBuildingTestDataAccess");
            var buildingRepo = new BuildingRepository(context);

            // Act
            buildingRepo.CreateBuilding(new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment> { apartment }
            });
            context.SaveChanges();

            // Assert
            Assert.IsTrue(buildingRepo.ExistApartment(apartment.Id));
        }

        [TestMethod]
        public void GetBuildingByNameTestCorrectDataAccess()
        {
            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
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

            var context = CreateDbContext("GetBuildingByNameTestCorrectDataAccess");
            var buildingRepo = new BuildingRepository(context);

            buildingRepo.CreateBuilding(building);
            context.SaveChanges();

            Assert.AreEqual(building, buildingRepo.GetBuildingByName(building.Name));
        }

        [TestMethod]
        public void GetAllBuildingsByManagerIdTestCorrectDataAccess()
        {
            User manager = new User {
                Name = "Manager",
                LastName = "Manager",
                Email = "acx@gail.com",
                Password = "1234",
                Role = Roles.Manager
            };
            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" }
                    }
                },
                Manager = manager
                
            };

            var context = CreateDbContext("GetAllBuildingsByManagerIdTestCorrectDataAccess");
            var buildingRepo = new BuildingRepository(context);

            buildingRepo.CreateBuilding(building);
            context.SaveChanges();

            Assert.AreEqual(1, buildingRepo.GetAllBuildingsByManager(manager.Id).Count());
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}