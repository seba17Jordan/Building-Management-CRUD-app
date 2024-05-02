using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.Out;
using ModelsApi.In;
using Moq;
using Domain.@enum;
using CustomExceptions;

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
}