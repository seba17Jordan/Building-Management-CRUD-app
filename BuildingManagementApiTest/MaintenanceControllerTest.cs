using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Moq;

namespace BuildingManagementApiTest;

[TestClass]
public class MaintenanceControllerTest
{
    [TestMethod]
    public void CreateMaintenancePersonCorrectTest()
    {
        //Arrange
        var maintenancePersonToCreate = new MaintenancePersonRequest
        {
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "strongPassword123"
        };

        var expectedMaintenancePerson = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "hashedPassword"
        };

        var expectedMaintenancePersonResponse = new MaintenancePersonResponse(expectedMaintenancePerson);

        var maintenancePersonLogicMock = new Mock<IUserLogic>();
        maintenancePersonLogicMock.Setup(m => m.CreateUser(It.IsAny<User>())).Returns(expectedMaintenancePerson);

        var maintenancePersonController = new MaintenancePersonController(maintenancePersonLogicMock.Object);

        // Act
        var result = maintenancePersonController.CreateMaintenancePerson(maintenancePersonToCreate) as CreatedAtActionResult;

        // Assert
        var createdMaintenancePerson = result.Value as MaintenancePersonResponse;
        Assert.IsNotNull(createdMaintenancePerson);
        Assert.AreEqual(expectedMaintenancePersonResponse, createdMaintenancePerson);
    }
}
