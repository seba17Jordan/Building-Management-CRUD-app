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
public class ServiceRequestControllerTest
{
    [TestMethod]
    public void CreateServiceRequestCorrectTest()
    {
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";

        User maintainanceUser = new User
        {
            Email = "mail@gmail.com",
            Role = Roles.Maintenance
        };

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
            CategoryId = category.Id,
            ApartmentId = apartment.Id,
            Status = ServiceRequestStatus.Open
        };

        var expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment,
            Category = category,
            Status = ServiceRequestStatus.Open
        };

        var expectedServiceRequestResponse = new ServiceRequestResponse(expectedServiceRequest);

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.CreateServiceRequest(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<User>())).Returns(expectedServiceRequest);
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(maintainanceUser);
        
        ServiceRequestController serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object, sessionService.Object);
        serviceRequestController.ControllerContext.HttpContext = new DefaultHttpContext();
        serviceRequestController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

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
    public void GetAllServiceRequestsManagerCorrectTest()//-------------------------------------------------------------------
    {
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";

        User manager = new User
        {
            Email = "akjn@gmail.com",
            Role = Roles.Manager
        };

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
               
                Apartment = apartment,
                
                Category = category,
                Status = ServiceRequestStatus.Open
            }
        };

        var expectedResponse = expectedServiceRequests.Select(sr => new ServiceRequestResponse(sr)).ToList();

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.GetAllServiceRequestsManager(It.IsAny<string>(),It.IsAny<Guid>())).Returns(expectedServiceRequests);
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(manager);

        var serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object, sessionService.Object);
        serviceRequestController.ControllerContext.HttpContext = new DefaultHttpContext();
        serviceRequestController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

        // Act
        var result = serviceRequestController.GetAllServiceRequestsManager("");

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
            
            Apartment = apartment,
            Category = category,
            
            Status = ServiceRequestStatus.Open
        };

        ServiceRequest expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment,
            Category = category,
            MaintenancePerson = maintenancePerson,
            Status = ServiceRequestStatus.Attending
        };

        IdRequest idRequest = new IdRequest { Id = maintenancePerson.Id };

        ServiceRequestResponse expectedServiceRequestResponse = new ServiceRequestResponse(expectedServiceRequest);

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.AssignRequestToMaintainancePerson(It.IsAny<Guid>(), It.IsAny<Guid>()))
        .Returns(expectedServiceRequest).Callback<Guid, Guid>((requestId, maintenancePersonId) =>
        {
            serviceRequest.Status = ServiceRequestStatus.Attending;
            serviceRequest.MaintenancePerson = maintenancePerson;
        });

        ServiceRequestController serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object, sessionService.Object);
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

    [TestMethod]
    public void UpdateServiceRequestToAttendingTestController()
    {
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e"; // Simula un token de autenticación válido

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
            Apartment = apartment,
            Category = category,
            MaintenancePerson = maintenancePerson,
            Status = ServiceRequestStatus.Open
        };

        ServiceRequest expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment,
            Category = category,
            MaintenancePerson = maintenancePerson,
            Status = ServiceRequestStatus.Attending,
            //fecha especifica
            StartDate = new DateTime(2024, 4, 30)
        };

        UpdateServiceRequestStatusRequest updateServiceRequestStatusRequest = new UpdateServiceRequestStatusRequest
        {};

        ServiceRequestResponse expectedServiceRequestResponse = new ServiceRequestResponse(expectedServiceRequest);

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.UpdateServiceRequestStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<decimal?>()))
        .Returns(expectedServiceRequest)
        .Callback((Guid requestId, Guid maintenancePersonId, decimal? totalCost) =>
        {
            expectedServiceRequest.Status = ServiceRequestStatus.Attending;
            expectedServiceRequest.EndDate = new DateTime(2024, 5, 30);
        });
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(maintenancePerson);


        ServiceRequestController serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object, sessionService.Object);
        serviceRequestController.ControllerContext.HttpContext = new DefaultHttpContext();
        serviceRequestController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedServiceRequestResponse);

        // Act
        var controllerResult = serviceRequestController.UpdateServiceRequestStatus(serviceRequest.Id, updateServiceRequestStatusRequest);

        // Assert
        serviceRequestLogic.VerifyAll();

        OkObjectResult resultObj = controllerResult as OkObjectResult;
        ServiceRequestResponse resultResponse = resultObj.Value as ServiceRequestResponse;

        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse, expectedServiceRequestResponse);
    }

    [TestMethod]
    public void UpdateServiceRequestToFinishedTestController()
    {
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
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
            Apartment = apartment,
            Category = category,
            MaintenancePerson = maintenancePerson,
            Status = ServiceRequestStatus.Attending,
            StartDate = new DateTime(2024, 4, 30)
        };

        ServiceRequest expectedServiceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment,
            Category = category,
            MaintenancePerson = maintenancePerson,
            Status = ServiceRequestStatus.Closed,
            TotalCost = 100,
            StartDate = new DateTime(2024, 4, 30),
            EndDate = new DateTime(2024, 5, 30)
        };

        UpdateServiceRequestStatusRequest updateServiceRequestStatusRequest = new UpdateServiceRequestStatusRequest { 
            TotalCost = 100
        };
        ServiceRequestResponse expectedServiceRequestResponse = new ServiceRequestResponse(expectedServiceRequest);

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.UpdateServiceRequestStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<decimal?>()))
        .Returns(expectedServiceRequest)
        .Callback((Guid requestId, Guid maintenancePersonId, decimal? totalCost) =>
        {
            expectedServiceRequest.Status = ServiceRequestStatus.Closed;
            expectedServiceRequest.TotalCost = 100;
            expectedServiceRequest.EndDate = new DateTime(2024, 5, 30);
        });
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(maintenancePerson);

        ServiceRequestController serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object, sessionService.Object);
        serviceRequestController.ControllerContext.HttpContext = new DefaultHttpContext();
        serviceRequestController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedServiceRequestResponse);

        // Act
        var controllerResult = serviceRequestController.UpdateServiceRequestStatus(serviceRequest.Id, updateServiceRequestStatusRequest);

        // Assert
        serviceRequestLogic.VerifyAll();

        OkObjectResult resultObj = controllerResult as OkObjectResult;
        ServiceRequestResponse resultResponse = resultObj.Value as ServiceRequestResponse;
        
        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse, expectedServiceRequestResponse);

    }

    [TestMethod]
    public void GetAllServiceRequestsMaintenanceCorrectTest()
    {
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
        User maintenancePerson = new User
        {
            Email = "maintenance@gmail.com",
            Password = "123456",
            Name = "John",
            LastName = "Doe",
            Role = Roles.Maintenance
        };

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

        ServiceRequest serviceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            Description = "A description",
            Apartment = apartment,
            Category = category,
            MaintenancePerson = maintenancePerson,
            Status = ServiceRequestStatus.Open
        };

        IEnumerable<ServiceRequest> expectedServiceRequests = new List<ServiceRequest>
        {
            serviceRequest
        };

        var expectedResponse = expectedServiceRequests.Select(sr => new ServiceRequestResponse(sr)).ToList();

        Mock<IServiceRequestLogic> serviceRequestLogic = new Mock<IServiceRequestLogic>(MockBehavior.Strict);
        Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        serviceRequestLogic.Setup(serviceRequestLogic => serviceRequestLogic.GetAllServiceRequestsMaintenance(It.IsAny<Guid>())).Returns(expectedServiceRequests);
        sessionService.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(maintenancePerson);

        ServiceRequestController serviceRequestController = new ServiceRequestController(serviceRequestLogic.Object, sessionService.Object);
        serviceRequestController.ControllerContext.HttpContext = new DefaultHttpContext();
        serviceRequestController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

        // Act
        var result = serviceRequestController.GetAllServiceRequestsMaintenance();

        // Assert
        serviceRequestLogic.VerifyAll();
        OkObjectResult resultObj = result as OkObjectResult;
        List<ServiceRequestResponse> resultResponse = resultObj.Value as List<ServiceRequestResponse>;
        Assert.IsNotNull(resultResponse);
        Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
        Assert.AreEqual(resultResponse.First(), expectedResponse.First());
    }
}