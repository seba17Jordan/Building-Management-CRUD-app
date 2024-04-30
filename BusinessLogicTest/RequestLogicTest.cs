using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
using Domain.@enum;
namespace BusinessLogicTest
{
    [TestClass]
    public class RequestLogicTest
    {
        [TestMethod]
        public void CreateServiceRequestCorrectTestLogic()
        {
            Category category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Category 1",
            };

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
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = category.Id,
                Apartment = apartment.Id
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns(expectedServiceRequest);
            buildingRepo.Setup(l => l.ExistApartment(It.IsAny<Guid>())).Returns(true);
            categoryRepo.Setup(l => l.FindCategoryById(It.IsAny<Guid>())).Returns(true);
            categoryRepo.Setup(l => l.GetCategoryById(It.IsAny<Guid>())).Returns(category);
            buildingRepo.Setup(l => l.GetApartmentById(It.IsAny<Guid>())).Returns(apartment);
            buildingRepo.Setup(l => l.GetBuildingIdByApartmentId(It.IsAny<Apartment>())).Returns(new Guid());

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object,buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            ServiceRequest logicResult = requestLogic.CreateServiceRequest(expectedServiceRequest);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateServiceRequestNullServiceRequestTestLogic()
        {
            //Arrange
            ServiceRequest expectedServiceRequest = null;

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns(expectedServiceRequest);

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            ServiceRequest logicResult = requestLogic.CreateServiceRequest(expectedServiceRequest);
            serviceRequestRepo.VerifyAll();
        }

        [TestMethod]
        public void AssigRequestToMaintainancePersonCorrectTestLogic()
        {
            //Arrange
            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = Guid.NewGuid(),
                Apartment = Guid.NewGuid(),
                Status = ServiceRequestStatus.Open
            };
            Guid maintainancePersonId = Guid.NewGuid();

            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = serviceRequest.Id,
                Description = serviceRequest.Description,
                Category = serviceRequest.Category,
                Apartment = serviceRequest.Apartment,
                Status = ServiceRequestStatus.Attending,
                MaintainancePersonId = maintainancePersonId
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            userRepository.Setup(l => l.GetUserById(It.IsAny<Guid>())).Returns(new User { Role = Roles.Maintenance });
            serviceRequestRepo.Setup(l => l.GetServiceRequestById(It.IsAny<Guid>())).Returns(serviceRequest);
            serviceRequestRepo.Setup(l => l.UpdateServiceRequest(It.IsAny<ServiceRequest>())).Callback<ServiceRequest>((ServiceRequest) =>
            {
                serviceRequest.Status = ServiceRequestStatus.Attending;
                serviceRequest.MaintainancePersonId = maintainancePersonId;
            });


            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            ServiceRequest logicResult = requestLogic.AssignRequestToMaintainancePerson(expectedServiceRequest.Id, maintainancePersonId);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }

        [TestMethod]
        public void GetAllServiceRequestsCorrectTestLogic()
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

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(repo => repo.GetAllServiceRequests(It.IsAny<string>())).Returns(expectedServiceRequests);

            RequestLogic serviceRequestController = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            IEnumerable<ServiceRequest> logicResult = serviceRequestController.GetAllServiceRequests("");

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequests);
        }

    }
}
