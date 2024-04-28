using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
namespace BusinessLogicTest
{
    [TestClass]
    public class BuildingLogicTest
    {
        [TestMethod]
        public void CreateBuildingCorrectTestLogic()
        {
            //Arrange
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

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            buildingRepo.Setup(l => l.CreateBuilding(It.IsAny<Building>())).Returns(expectedBuilding);
            buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

            //Act
            Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            //Assert
            buildingRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedBuilding);
        }

        [TestMethod]
        public void CreateBuildingNullShouldThrowNullExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            try
            {   
                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(null);

            }
            catch (ArgumentNullException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentNullException));   //Crear exception especifica
            Assert.IsTrue(specificEx.Message.Contains("Building is null"));
        }

        [TestMethod]
        public void CreateBuildingInvalidNameThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "",
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
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building invalid data"));
        }

        [TestMethod]
        public void CreateBuildingInvalidAddressThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "",
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
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building invalid data"));
        }

        [TestMethod]
        public void CreateBuildingInvalidCompanyThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = "",
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
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building invalid data"));
        }

        [TestMethod]
        public void CreateBuildingNoApartmentsThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = "Company",
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>()
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building must have at least one apartment"));
        }

        [TestMethod]
        public void CreateBuildingInvalidExpensesThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = "Company",
                    CommonExpenses = -100,
                    Apartments = new List<Apartment>()
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Common expenses must be greater than 0"));
        }

        [TestMethod]
        public void CreateBuildingRepeatedNameThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = "Company",
                    CommonExpenses = -100,
                    Apartments = new List<Apartment>()
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(true);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building with same name already exists"));
        }

        [TestMethod]
        public void DeleteBuildingByIdCorrectTestLogic()
        {
            // Arrange
            Guid expectedId = Guid.NewGuid();
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            buildingRepo.Setup(l => l.DeleteBuilding(It.IsAny<Building>()));
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(new Building());
            buildingRepo.Setup(l => l.Save());
            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

            // Act
            buildingLogic.DeleteBuildingById(expectedId);

            // Assert
            buildingRepo.VerifyAll();
        }

        [TestMethod]
        public void DeleteBuildingByIdEmptyIdShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

            try
            {
                // Act
                buildingLogic.DeleteBuildingById(Guid.Empty);
            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Id is empty"));
        }

        [TestMethod]
        public void DeleteBuildingByIdNotFoundShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns((Building)null);
            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

            try
            {
                // Act
                buildingLogic.DeleteBuildingById(Guid.NewGuid());
            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building not found"));
        }

        [TestMethod]
        public void UpdateBuildingNameTestLogic()
        {
            //Arrange
            Building building = new Building()
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

            Building updates = new Building()
            {
                Name = "Building 2",
            };

            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 2",
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

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(building);
            buildingRepo.Setup(l => l.UpdateBuilding(It.IsAny<Building>()));
            buildingRepo.Setup(l => l.Save());

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

            //Act
            Building logicResult = buildingLogic.UpdateBuildingById(building.Id, updates);

            //Assert
            buildingRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedBuilding);
        }
    }
}
