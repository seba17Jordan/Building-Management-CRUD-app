using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.Out;
using ModelsApi.In;
using Moq;

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
            Password = "strongPassword123"
        };

        var expectedAdmin = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "hashedPassword"
        };

        var expectedAdminResponse = new AdministratorResponse(expectedAdmin);

        Mock<IAdministratorLogic> administratorLogic = new Mock<IAdministratorLogic>(MockBehavior.Strict);
        administratorLogic.Setup(x => x.CreateAdministrator(It.IsAny<User>())).Returns(expectedAdmin);
        var administratorController = new AdministratorController(administratorLogic.Object);

        var expectedObjAdmin = new CreatedAtActionResult("CreateAdministrator", "CreateAdministrator", new { id = 1 }, expectedAdminResponse);

        // Act
        var result = administratorController.CreateAdministrator(adminToCreate) as CreatedAtActionResult;

        // Assert
        administratorLogic.VerifyAll();

        AdministratorResponse createdAdmin = result.Value as AdministratorResponse;
        Assert.AreEqual(result.StatusCode, expectedObjAdmin.StatusCode);
        Assert.AreEqual(expectedAdminResponse, createdAdmin);
    }
}