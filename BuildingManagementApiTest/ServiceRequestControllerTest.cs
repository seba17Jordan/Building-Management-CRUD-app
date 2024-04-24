using BuildingManagementApi.Controllers;
using Domain;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Moq;

namespace BuildingManagementApiTest;

[TestClass]
public class ServiceRequestControllerTest
{
    [TestMethod]
    public void CreateServiceRequestCorrectTest()
    {
        //Arrange
        var serviceRequestToCreate = new ServiceRequestRequest
        {
            Description = "A description",
            Apartment = new ApartmentRequest {
                Bathrooms = 2,
                Floor = 2,
                HasTerrace = true,
                Number = 2,
                Rooms = 2,
                Owner = new OwnerRequest { Email = "example@gmail.com", LastName = "Doe", Name = "John" }
            },
            Category = new CategoryRequest { Name = "Plumbing" },
            Status = ServiceRequestStatus.Open
        };

        var expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),  
            Description = "A description",
            Apartment = new Apartment
            {
                Bathrooms = 2,
                Floor = 2,
                HasTerrace = true,
                Number = 2,
                Rooms = 2,
                Owner = new Owner { Email = "example@gmail.com", LastName = "Doe", Name = "John" }
            },
            Category = new Category { Name = "Plumbing" },
            Status = ServiceRequestStatus.Open
        };

        var expectedServiceRequestResponse = new ServiceRequestResponse(expectedServiceRequest);

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns(expectedServiceRequest);
        var serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object);

        var expectedObjServiceRequest = new CreatedAtActionResult("CreateServiceRequest", "CreateServiceRequest", new { id = 1 }, expectedServiceRequestResponse);

        //Act
        var result = serviceRequestController.CreateServiceRequest(serviceRequestToCreate);

        //Assert
        serviceRequestLogic.VerifyAll();

        CreatedAtActionResult resultObject = result as CreatedAtActionResult;
        ServiceRequestResponse resultResponse = resultObject.Value as ServiceRequestResponse;

        Assert.AreEqual(resultObject.StatusCode, expectedObjServiceRequest.StatusCode);
        Assert.AreEqual(resultResponse, expectedServiceRequestResponse);
    }
}