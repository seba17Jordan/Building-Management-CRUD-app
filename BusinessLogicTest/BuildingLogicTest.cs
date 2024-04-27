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

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object);

            //Act
            Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            //Assert
            buildingRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedBuilding);
        }

        [TestMethod]
        public void CreateBuildingNullShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            try
            {
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Building 2",
                    Address = "Address 2",
                    ConstructionCompany = "Construction Company 1",
                    CommonExpenses = 200,
                    Apartments = new List<Apartment>
                    {
                        new Apartment()
                        {
                            Floor = 2,
                            Number = 202,
                            Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                            Rooms = 2,
                            Bathrooms = 2,
                            HasTerrace = true
                        }
                    }
                };
                
                buildingRepo.Setup(repo => repo.CreateBuilding(It.IsAny<Building>())).Returns(expectedBuilding);
                
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
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));   //Crear exception especifica
            Assert.AreEqual("Error", specificEx.Message);
        }
    }
}
