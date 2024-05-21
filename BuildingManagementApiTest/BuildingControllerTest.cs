using BuildingManagementApi.Controllers;
using Domain;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Moq;
using Newtonsoft.Json.Linq;

namespace BuildingManagementApiTest;



[TestClass]
public class BuildingControllerTest
{

    private Mock<IBuildingLogic> _buildingLogicMock;
    private BuildingController _controller;
    private ISessionService _sessionService;

    [TestInitialize]
    public void Setup()
    {
        _buildingLogicMock = new Mock<IBuildingLogic>();
        _controller = new BuildingController(_buildingLogicMock.Object, _sessionService);
    }

    [TestMethod]
    public void CreateBuildingCorrectTest()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
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

        ConstructionCompany constructionCom = new ConstructionCompany("Construction Company");

        Building expectedBuilding = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building",
            Address = "Address 1",
            ConstructionCompany = constructionCom,
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
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        buildingLogic.Setup(buildingLogic => buildingLogic.CreateBuilding(It.IsAny<Building>())).Returns(expectedBuilding);
        sessionService.Setup(sessionService => sessionService.GetUserByToken(It.IsAny<Guid>())).Returns(new User { Id = Guid.NewGuid() });
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        CreatedAtActionResult expectedObjResult = new CreatedAtActionResult("CreateBuilding", "CreateBuilding", new {id = 3 } , expectedMappedBuilding);

        //Act
        var finalResult = buildingController.CreateBuilding(buildingToCreate);

        //Assert
        buildingLogic.VerifyAll();

        CreatedAtActionResult resultObject = finalResult as CreatedAtActionResult;
        BuildingResponse resultValue = resultObject.Value as BuildingResponse;

        Assert.AreEqual(resultObject.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultValue, expectedMappedBuilding);
        Assert.AreEqual(resultValue, expectedMappedBuilding);

    }

    [TestMethod]
    public void DeleteBuildingByIdCorrectTestController()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
        User manager = new User { Id = Guid.NewGuid(), Role = Domain.@enum.Roles.Manager };
        ConstructionCompany constructionCom = new ConstructionCompany("Construction Company");

        Building buildingToDelete = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building",
            Address = "Address 1",
            ConstructionCompany = constructionCom,
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
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        buildingLogic.Setup(buildingLogic => buildingLogic.DeleteBuildingById(buildingToDelete.Id, manager.Id));
        sessionService.Setup(sessionService => sessionService.GetUserByToken(It.IsAny<Guid>())).Returns(manager);
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        var expectedObjResult = new NoContentResult();

        //Act
        var finalResult = buildingController.DeleteBuildingById(buildingToDelete.Id);

        //Assert
        buildingLogic.VerifyAll();

        NoContentResult resultObject = finalResult as NoContentResult;

        Assert.IsNotNull(resultObject);
    }

    [TestMethod]
    public void UpdateBuildingCorrectTestController()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
        ConstructionCompany constructionCom = new ConstructionCompany("Construction Company");

        Building building = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building 1",
            Address = "Address 1",
            ConstructionCompany = constructionCom,
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

        ConstructionCompany constructionCom2 = new ConstructionCompany("Construction Company 2");

        BuildingRequest buildingUpdates = new BuildingRequest()
        {
            Name = "New Building 2",
            Address = "Address 2",
            ConstructionCompany = "Construction Company 2",
            CommonExpenses = 200
        };

        Building expectedBuilding = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building 2",
            Address = "Address 2",
            ConstructionCompany = constructionCom,
            CommonExpenses = 200,
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
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        buildingLogic.Setup(bl => bl.UpdateBuildingById(It.IsAny<Guid>(), It.IsAny<Building>(), It.IsAny<Guid>())).Returns(expectedBuilding);
        sessionService.Setup(ss => ss.GetUserByToken(It.IsAny<Guid>())).Returns(new User { Id = Guid.NewGuid() });

        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedMappedBuilding);

        //Act
        var finalResult = buildingController.UpdateBuildingById(building.Id, buildingUpdates);

        //Assert
        buildingLogic.VerifyAll();

        OkObjectResult resultObject = finalResult as OkObjectResult;
        BuildingResponse resultValue = resultObject.Value as BuildingResponse;

        Assert.AreEqual(resultObject.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultValue, expectedMappedBuilding);
    }

    [TestMethod]
    public void ModifyBuildingManagerCorrectTestController()
    {
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";

        //Arrange
        ConstructionCompany constructionCom = new ConstructionCompany("Construction Company");

        User constructionCompanyAdmin = new User
        { 
            Id = Guid.NewGuid(),
            Role = Roles.ConstructionCompanyAdmin,
            Name = "John",
            LastName = "Doe"
        };
        User newManager = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.Manager,
            Name = "ASSAS",
            LastName = "sssA"
        };

        IdRequest newBuildingManagerId = new IdRequest { Id = newManager.Id };

        Building building = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building 1",
            Address = "Address 1",
            ConstructionCompany = constructionCom,
            ConstructionCompanyAdmin = constructionCompanyAdmin,
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

        Building expectedBuilding = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building 1",
            Address = "Address 1",
            ConstructionCompany = constructionCom,
            ConstructionCompanyAdmin = constructionCompanyAdmin,
            managerId = newManager.Id,
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

        BuildingResponse expectedMappedBuilding = new BuildingResponse(expectedBuilding);

        Mock<IBuildingLogic> buildingLogic = new Mock<IBuildingLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        sessionService.Setup(sessionService => sessionService.GetUserByToken(It.IsAny<Guid>())).Returns(constructionCompanyAdmin);
        buildingLogic.Setup(buildingLogic => buildingLogic.ModifyBuildingManager(building.Id, newManager.Id, constructionCompanyAdmin.Id)).Returns(expectedBuilding);
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedMappedBuilding);

        //Act
        var finalResult = buildingController.ModifyBuildingManager(building.Id, newBuildingManagerId);

        //Assert
        buildingLogic.VerifyAll();

        OkObjectResult resultObject = finalResult as OkObjectResult;
        BuildingResponse resultValue = resultObject.Value as BuildingResponse;

        Assert.AreEqual(resultObject.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultValue, expectedMappedBuilding);
    }

    /*
    [TestMethod]
    public void CreateConstructionCompanyTest()
    {
        // Arrange
        var validToken = Guid.Parse("b4d9e6a4-466c-4a4f-91ea-6d7e7997584e");
        var constructionCompanyRequest = new ConstructionCompanyRequest
        {
            Name = "Test"
        };


        var constructionCompanyAdmin = new User
        {
            Id = Guid.NewGuid(),
            Email = "lkand@gmail.com",
            Password = "1234",
            Role = Roles.ConstructionCompanyAdmin,
            Name = "Lukas",
            LastName = "Kand"
        };

        var _sessionServiceMock = new Mock<ISessionService>();
        _sessionServiceMock.Setup(x => x.GetUserByToken(validToken)).Returns(constructionCompanyAdmin);


        var _constructionCompanyLogicMock = new Mock<IConstructionCompanyLogic>();
        _constructionCompanyLogicMock.Setup(x => x.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(new ConstructionCompany());


        var _controller = new ConstructionCompanyController(_constructionCompanyLogicMock.Object, _sessionServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        var httpRequestMock = new Mock<HttpRequest>();
        httpRequestMock.SetupGet(x => x.Headers).Returns(new HeaderDictionary { { "Authorization", validToken.ToString() } });
        httpContextMock.SetupGet(x => x.Request).Returns(httpRequestMock.Object);
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock.Object };

        // Act
        var result = _controller.CreateConstructionCompany(constructionCompanyRequest) as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(201, result.StatusCode);
        Assert.AreEqual("CreateConstructionCompany", result.ActionName);
    }*/

}