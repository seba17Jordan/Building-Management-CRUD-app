using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Moq;

namespace BuildingManagementApiTest;

[TestClass]
public class BuildingControllerTest
{
    [TestMethod]
    public void CreateBuildingCorrectTest()
    {
        //Arrange
        BuildingRequest buildingToCreate = new BuildingRequest()
        {
            Name = "New Building",
            Address = "Address 1",
            ConstructionCompany = "Construction Company",
            CommonExpenses = 100,
            Apartments = new List<ApartmentRequest>
            {
                new ApartmentRequest()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new OwnerRequest { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                    Rooms = 3,
                    Bathrooms = 2,
                    HasTerrace = true
                }
            }
        };

        Building expectedBuilding = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building",
            Address = "Address 1",
            ConstructionCompany = "Construction Company",
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

        var expectedMappedBuilding = new BuildingResponse(expectedBuilding); 

        Mock<IBuildingLogic> buildingLogic = new Mock<IBuildingLogic>(MockBehavior.Strict);
        buildingLogic.Setup(buildingLogic => buildingLogic.CreateBuilding(It.IsAny<Building>())).Returns(expectedBuilding);
        var buildingController = new BuildingController(buildingLogic.Object);

        var expectedObjResult = new CreatedAtActionResult("CreateBuilding", "CreateBuilding", new {id = 3 } , expectedMappedBuilding);

        //Act
        var finalResult = buildingController.CreateBuilding(buildingToCreate);

        //Assert
        buildingLogic.VerifyAll();

        CreatedAtActionResult resultObject = finalResult as CreatedAtActionResult;
        BuildingResponse resultValue = resultObject.Value as BuildingResponse;

        Assert.AreEqual(resultObject.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultValue, expectedMappedBuilding);

    }

    [TestMethod]
    public void DeleteBuildingByIdCorrectTest()
    {
        //Arrange
        Building buildingToDelete = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building",
            Address = "Address 1",
            ConstructionCompany = "Construction Company",
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

        Mock<IBuildingLogic> buildingLogic = new Mock<IBuildingLogic>(MockBehavior.Strict);
        buildingLogic.Setup(buildingLogic => buildingLogic.DeleteBuilding(buildingToDelete.Id));
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object);

        var expectedObjResult = new NoContentResult();

        //Act
        var finalResult = buildingController.DeleteBuildingById(buildingToDelete.Id);

        //Assert
        buildingLogic.VerifyAll();

        NoContentResult resultObject = finalResult as NoContentResult;

        Assert.IsNotNull(resultObject);
    }
}