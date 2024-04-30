﻿using BuildingManagementApi.Controllers;
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
        Category category = new Category { Name = "Category 1" };

        Apartment apartment = new Apartment()
        {
            Floor = 1,
            Number = 101,
            Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
            Rooms = 3,
            Bathrooms = 2,
            HasTerrace = true
        };
        //Arrange
        var serviceRequestToCreate = new ServiceRequestRequest
        {
            Description = "A description",
            Apartment = apartment.Id,
            Category = category.Id,
            Status = ServiceRequestStatus.Open
        };

        var expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),  
            Description = "A description",
            Apartment = apartment.Id,
            Category = category.Id,
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

    [TestMethod]
    public void GetAllServiceRequestsCorrectTest()
    {
        Category category = new Category { Name = "Category 1" };

        Apartment apartment = new Apartment()
        {
            Floor = 1,
            Number = 101,
            Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
            Rooms = 3,
            Bathrooms = 2,
            HasTerrace = true
        };

        //Arrange
        IEnumerable<ServiceRequest> expectedServiceRequests = new List<ServiceRequest>
        {
            new ServiceRequest
            {
                Id = Guid.NewGuid(),
                Description = "A description",
                Apartment = apartment.Id,
                Category = category.Id,
                Status = ServiceRequestStatus.Open
            }
        };

        var expectedResponse = expectedServiceRequests.Select(sr => new ServiceRequestResponse(sr)).ToList();

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.GetAllServiceRequests(It.IsAny<string>())).Returns(expectedServiceRequests);
        
        var serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object);
        
        OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

        // Act
        var result = serviceRequestController.GetAllServiceRequests("");

        // Assert
        serviceRequestLogic.VerifyAll();

        OkObjectResult resultObj = result as OkObjectResult;
        List<ServiceRequestResponse> resultResponse = resultObj.Value as List<ServiceRequestResponse>;

        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse.First(), expectedResponse.First());
    }

    [TestMethod]
    public void AssignRequestToMaintainancePersonCorrectTestController()
    {
        Category category = new Category { Name = "Category 1" };

        Apartment apartment = new Apartment()
        {
            Floor = 1,
            Number = 101,
            Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" },
            Rooms = 3,
            Bathrooms = 2,
            HasTerrace = true
        };

        User maintenancePerson = new User
        {
            Email = "maintainance@gmail.com",
            Password = "123456",
            Name = "John",
            LastName = "Doe",
            Role = Roles.Maintenance
        };

        ServiceRequest serviceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment.Id,
            Category = category.Id,
            Status = ServiceRequestStatus.Open
        };

        ServiceRequest expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment.Id,
            Category = category.Id,
            MaintainancePersonId = maintenancePerson.Id,
            Status = ServiceRequestStatus.Attending
        };

        IdRequest idRequest = new IdRequest { Id = maintenancePerson.Id };

        ServiceRequestResponse expectedServiceRequestResponse = new ServiceRequestResponse(expectedServiceRequest);

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.AssignRequestToMaintainancePerson(It.IsAny<Guid>(), It.IsAny<Guid>()))
        .Returns(expectedServiceRequest).Callback<Guid, Guid>((requestId, maintenancePersonId) =>
        {
            serviceRequest.Status = ServiceRequestStatus.Attending;
            serviceRequest.MaintainancePersonId = maintenancePersonId;
        });

        ServiceRequestController serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object);
        OkObjectResult expectedObjResult = new OkObjectResult(expectedServiceRequestResponse);

        // Act
        var controllerResult = serviceRequestController.AssignRequestToMaintainancePerson(serviceRequest.Id, idRequest);

        // Assert
        serviceRequestLogic.VerifyAll();

        OkObjectResult resultObj = controllerResult as OkObjectResult;
        ServiceRequestResponse resultResponse = resultObj.Value as ServiceRequestResponse;

        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse, expectedServiceRequestResponse);
    }
}