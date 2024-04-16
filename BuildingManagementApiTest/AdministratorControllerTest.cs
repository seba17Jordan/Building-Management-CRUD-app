using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
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
            Lastname = "Doe",
            Email = "john.doe@example.com",
            Password = "strongPassword123"
        };

        var expectedAdmin = new Administrator
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Lastname = "Doe",
            Email = "john.doe@example.com",
            Password = "hashedPassword"
        };

        var expectedAdminResponse = new AdministratorResponse(expectedAdmin);

        var administratorLogicMock = new Mock<IAdministratorLogic>();
        administratorLogicMock.Setup(x => x.CreateAdministrator(It.IsAny<Administrator>())).Returns(expectedAdmin);

        var administratorController = new AdministratorController(administratorLogicMock.Object);

        // Act
        var result = administratorController.CreateAdministrator(adminToCreate) as CreatedAtActionResult;

        // Assert
        var createdAdmin = result.Value as AdministratorResponse;
        Assert.IsNotNull(createdAdmin);
        Assert.AreEqual(expectedAdminResponse.Id, createdAdmin.Id);
    }
}