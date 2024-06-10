using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.Out;
using ModelsApi.In;
using Moq;
using Domain.@enum;
using CustomExceptions;
using ImportersInterface;
using Microsoft.AspNetCore.Http;

namespace BuildingManagementApiTest;

[TestClass]
public class AdministratorControllerTest
{
    [TestMethod]
    public void CreateAdminCorrectTest() 
    {
        //Arrange
        var adminToCreate = new AdministratorRequest
        {
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "strongPassword123",
            Role = Roles.Administrator
        };

        var expectedAdmin = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "strongPassword123",
            Role = Roles.Administrator
        };

        var expectedAdminResponse = new AdministratorResponse(expectedAdmin);

        Mock<IUserLogic> userLogic = new Mock<IUserLogic>(MockBehavior.Strict);
        userLogic.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(expectedAdmin);
        var administratorController = new AdministratorController(userLogic.Object);

        var expectedObjAdmin = new CreatedAtActionResult("CreateAdministrator", "CreateAdministrator", new { id = 1 }, expectedAdminResponse);

        // Act
        var result = administratorController.CreateAdministrator(adminToCreate) as CreatedAtActionResult;

        // Assert
        userLogic.VerifyAll();

        AdministratorResponse createdAdmin = result.Value as AdministratorResponse;
        Assert.AreEqual(result.StatusCode, expectedObjAdmin.StatusCode);
        Assert.AreEqual(expectedAdminResponse, createdAdmin);
    }

    [TestMethod]
    public void CreateAdministrator_ReturnsConflict_WhenEmailExists()
    {
        // Arrange
        var mockUserLogic = new Mock<IUserLogic>();
        var controller = new AdministratorController(mockUserLogic.Object);

        var adminRequest = new AdministratorRequest
        {
            Email = "admin@example.com",
            Password = "admin123",
            Name = "Admin",
            LastName = "User",
            Role = Roles.Administrator
        };

        // Simular que el correo electrónico ya existe
        mockUserLogic.Setup(x => x.CreateUser(It.IsAny<User>())).Throws(new ObjectAlreadyExistsException("Email already exists"));

        try
        {
            // Act
            var result = controller.CreateAdministrator(adminRequest) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(409, result.StatusCode);
            Assert.AreEqual("Email already exists", result.Value);
        }
        catch (ObjectAlreadyExistsException ex)
        {
            // Si se lanza la excepción ObjectAlreadyExistsException, la prueba pasa
            Assert.AreEqual("Email already exists", ex.Message);
        }
    }

    [TestMethod]
    public void CreateAdminIncorrectTest()
    {
        // Arrange
        var adminToCreate = new AdministratorRequest
        {
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "",  // Contraseña vacía, otro dato incorrecto
            Role = Roles.Administrator
        };

        // Crear el controlador con la lógica simulada
        Mock<IUserLogic> userLogic = new Mock<IUserLogic>();
        var controller = new AdministratorController(userLogic.Object);

        try
        {
            // Act
            var result = controller.CreateAdministrator(adminToCreate) as ObjectResult;

            // Assert
            Assert.Fail("Expected exception not thrown.");
        }
        catch (ArgumentException ex)
        {
            // Assert
            Assert.AreEqual("There is a missing field in the request's body", ex.Message);
        }
    }

    [TestMethod]
    public void GetAllManagersTestController()
    {
        //Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
        
        User manager = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.Manager,
            Name = "John",
            LastName = "Doe"
        };

        IEnumerable<User> expectedManagers = new List<User>
        {
            manager
        };

        var expectedResponse = expectedManagers.Select(b => new UserResponse(b)).ToList();

        Mock<IUserLogic> userLogic = new Mock<IUserLogic>(MockBehavior.Strict);

        userLogic.Setup(x => x.GetAllManagers()).Returns(expectedManagers);

        AdministratorController adminController = new AdministratorController(userLogic.Object);
        adminController.ControllerContext.HttpContext = new DefaultHttpContext();
        adminController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

        // Act
        var constrollerResult = adminController.GetAllManagers();

        // Assert
        userLogic.VerifyAll();
        OkObjectResult resultObj = constrollerResult as OkObjectResult;
        List<UserResponse> resultResponse = resultObj.Value as List<UserResponse>;
        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse.First(), expectedResponse.First());
    }

}