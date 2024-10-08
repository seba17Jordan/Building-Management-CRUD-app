using BuildingManagementApi.Controllers;
using Domain;
using Domain.@enum;
using ImportersInterface;
using ImportersLogic;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
    private Mock<IBuildingService> _buildingServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _buildingLogicMock = new Mock<IBuildingLogic>(); 
        
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
        Mock<IBuildingService> buildingService = new Mock<IBuildingService>(MockBehavior.Strict);

        buildingLogic.Setup(buildingLogic => buildingLogic.CreateBuilding(It.IsAny<Building>(), It.IsAny<User>())).Returns(expectedBuilding);
        sessionService.Setup(sessionService => sessionService.GetUserByToken(It.IsAny<Guid>())).Returns(new User { Id = Guid.NewGuid() });
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object, buildingService.Object);
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
        Mock<IBuildingService> buildingService = new Mock<IBuildingService>(MockBehavior.Strict);

        buildingLogic.Setup(buildingLogic => buildingLogic.DeleteBuildingById(buildingToDelete.Id, manager.Id));
        sessionService.Setup(sessionService => sessionService.GetUserByToken(It.IsAny<Guid>())).Returns(manager);
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object, buildingService.Object);
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
        Mock<IBuildingService> buildingService = new Mock<IBuildingService>(MockBehavior.Strict);

        buildingLogic.Setup(bl => bl.UpdateBuildingById(It.IsAny<Guid>(), It.IsAny<Building>(), It.IsAny<Guid>())).Returns(expectedBuilding);
        sessionService.Setup(ss => ss.GetUserByToken(It.IsAny<Guid>())).Returns(new User { Id = Guid.NewGuid() });

        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object, buildingService.Object);
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
            Manager = newManager,
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
        Mock<IBuildingService> buildingService = new Mock<IBuildingService>(MockBehavior.Strict);

        sessionService.Setup(sessionService => sessionService.GetUserByToken(It.IsAny<Guid>())).Returns(constructionCompanyAdmin);
        buildingLogic.Setup(buildingLogic => buildingLogic.ModifyBuildingManager(building.Id, newManager.Id, constructionCompanyAdmin.Id)).Returns(expectedBuilding);
        
        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object, buildingService.Object);
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

    [TestMethod]
    public void GetAllBuildingsByAdminCorrectTestController()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
        ConstructionCompany constructionCom = new ConstructionCompany("Construction Company");

        User constructionCompanyAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ConstructionCompanyAdmin,
            Name = "John",
            LastName = "Doe"
        };

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

        IEnumerable<Building> expectedBuildings = new List<Building>
        {
            building
        };

        var expectedResponse = expectedBuildings.Select(b => new BuildingResponse(b)).ToList();

        Mock<IBuildingLogic> buildingLogic = new Mock<IBuildingLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        Mock<IBuildingService> buildingService = new Mock<IBuildingService>(MockBehavior.Strict);

        buildingLogic.Setup(bl => bl.GetBuildingsByCompanyAdminId(It.IsAny<Guid>())).Returns(expectedBuildings);
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(constructionCompanyAdmin);

        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object, buildingService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

        // Act
        var constrollerResult = buildingController.GetAllBuildingsCompanyAdmin();

        // Assert
        buildingLogic.VerifyAll();
        sessionService.VerifyAll();
        OkObjectResult resultObj = constrollerResult as OkObjectResult;
        List<BuildingResponse> resultResponse = resultObj.Value as List<BuildingResponse>;
        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse.First(), expectedResponse.First());
    }

    [TestMethod]
    public void GetAllBuildingsByManagerCorrectTestController()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
        ConstructionCompany constructionCom = new ConstructionCompany("Construction Company");

        User manager = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.Manager,
            Name = "John",
            LastName = "Doe"
        };

        User constructionCompanyAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ConstructionCompanyAdmin,
            Name = "John",
            LastName = "Doe"
        };

        Building building = new Building()
        {
            Id = Guid.NewGuid(),
            Name = "New Building 1",
            Address = "Address 1",
            ConstructionCompany = constructionCom,
            ConstructionCompanyAdmin = constructionCompanyAdmin,
            CommonExpenses = 100,
            Manager = manager,
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

        IEnumerable<Building> expectedBuildings = new List<Building>
        {
            building
        };

        var expectedResponse = expectedBuildings.Select(b => new BuildingResponse(b)).ToList();

        Mock<IBuildingLogic> buildingLogic = new Mock<IBuildingLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        Mock<IBuildingService> buildingService = new Mock<IBuildingService>(MockBehavior.Strict);

        buildingLogic.Setup(bl => bl.GetBuildingsByManagerId(It.IsAny<Guid>())).Returns(expectedBuildings);
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(constructionCompanyAdmin);

        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object, buildingService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

        // Act
        var constrollerResult = buildingController.GetAllBuildingsByManager();

        // Assert
        buildingLogic.VerifyAll();
        sessionService.VerifyAll();
        OkObjectResult resultObj = constrollerResult as OkObjectResult;
        List<BuildingResponse> resultResponse = resultObj.Value as List<BuildingResponse>;
        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse.First(), expectedResponse.First());
    }

    /*
    [TestMethod]
    public void ImportBuildingCorrectControllerTest()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";

        User constructionCompanyAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ConstructionCompanyAdmin,
            Name = "John",
            LastName = "Doe"
        };

        ConstructionCompany constructionCom = new ConstructionCompany()
        {
            Name = "Construction Company",
            ConstructionCompanyAdmin = constructionCompanyAdmin
        };

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

        ImportRequest importRequest = new ImportRequest()
        {
            ImporterName = "JsonImporter.dll",
            FileName = "FileName.json"
        };

        Mock<IBuildingImporter> buildingImporter = new Mock<IBuildingImporter>(MockBehavior.Strict);
        Mock<IBuildingLogic> buildingLogic = new Mock<IBuildingLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        Mock<ImporterManager> importerManager = new Mock<ImporterManager>(MockBehavior.Strict);
       
        buildingImporter.Setup(bi => bi.Import(It.IsAny<string>()));
        importerManager.Setup(im => im.GetImporter(It.IsAny<string>(), It.IsAny<string>())).Returns(buildingImporter.Object);

        BuildingController buildingController = new BuildingController(buildingLogic.Object, sessionService.Object);
        buildingController.ControllerContext.HttpContext = new DefaultHttpContext();
        buildingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        //Act
        var controllerResult = buildingController.ImportBuildings(importRequest) as OkResult;

        //Assert
        Assert.AreEqual(200, controllerResult.StatusCode);
    }*/

}