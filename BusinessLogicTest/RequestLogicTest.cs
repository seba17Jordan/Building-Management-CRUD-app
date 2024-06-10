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
            // Arrange

            Category category = new Category()
            {
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

            User manager = new User()
            {
                Role = Roles.Manager
            };

            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = category,
                Apartment = apartment,
                Status = ServiceRequestStatus.Open,
                Building = new Building
                {
                    Address = "Address 1",
                    Name = "Building 1",
                    Apartments = new List<Apartment> { apartment }
                },
                Manager = manager
            };

            var building = new Building()
            {
                Address = "Address 1",
                Name = "Building 1",
                Apartments = new List<Apartment> { apartment }
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns((ServiceRequest req) => req);
            buildingRepo.Setup(l => l.ExistApartment(apartment.Id)).Returns(true);
            buildingRepo.Setup(l => l.GetBuildingById(building.Id)).Returns(building);
            categoryRepo.Setup(l => l.FindCategoryById(category.Id)).Returns(true);
            categoryRepo.Setup(l => l.GetCategoryById(category.Id)).Returns(category);
            buildingRepo.Setup(l => l.GetApartmentById(apartment.Id)).Returns(apartment);
            buildingRepo.Setup(l => l.GetBuildingIdByApartment(apartment)).Returns(building.Id);

            var requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            // Act
            ServiceRequest logicResult = null;
            try
            {
                logicResult = requestLogic.CreateServiceRequest(apartment.Id, category.Id, expectedServiceRequest.Description, manager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
                throw;
            }

            // Assert
            serviceRequestRepo.VerifyAll();
            Assert.IsNotNull(logicResult);
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }

        [TestMethod]
        public void AssigRequestToMaintainancePersonCorrectTestLogic()
        {
            Category category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Category 1",
            };

            Apartment apartment = new Apartment()
            {
                Id = Guid.NewGuid(),
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" }
            };
            //Arrange
            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = category,
                Apartment = apartment,
                Status = ServiceRequestStatus.Open
            };
            User maintenancePerons = new User()
            {
                Role = Roles.Maintenance
            };

            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = serviceRequest.Id,
                Description = serviceRequest.Description,
                Category = serviceRequest.Category,
                Apartment = serviceRequest.Apartment,
                Status = ServiceRequestStatus.Attending,
                MaintenancePerson = maintenancePerons
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
                serviceRequest.MaintenancePerson = maintenancePerons;
            });


            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            ServiceRequest logicResult = requestLogic.AssignRequestToMaintainancePerson(expectedServiceRequest.Id, maintenancePerons.Id);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }

        [TestMethod]
        public void GetAllServiceRequestsCorrectTestLogic()
        {
            User manager = new User()
            {
                Id = Guid.NewGuid(),
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

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(repo => repo.GetAllServiceRequestsManager(It.IsAny<string>(), It.IsAny<Guid>())).Returns(expectedServiceRequests);

            RequestLogic serviceRequestController = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            IEnumerable<ServiceRequest> logicResult = serviceRequestController.GetAllServiceRequestsManager("", manager.Id);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequests);
        }

        [TestMethod]
        public void UpdateServiceRequestToAcceptedCorrectTestLogic()
        {
            //Arrange
            User maintenancePerson = new User()
            {
                Id = Guid.NewGuid(),
                Role = Roles.Maintenance
            };

            Category category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Category 1",
            };

            Apartment apartment = new Apartment()
            {
                Id = Guid.NewGuid(),
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" }
            };
            
            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = category,
                Apartment = apartment,
                Status = ServiceRequestStatus.Open,
                MaintenancePerson = maintenancePerson
            };

            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = serviceRequest.Id,
                Description = serviceRequest.Description,
                Category = serviceRequest.Category,
                Apartment = serviceRequest.Apartment,
                Status = ServiceRequestStatus.Attending,
                MaintenancePerson = maintenancePerson
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.UpdateServiceRequest(It.IsAny<ServiceRequest>()));
            serviceRequestRepo.Setup(l => l.GetServiceRequestById(It.IsAny<Guid>())).Returns(serviceRequest);

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            ServiceRequest logicResult = requestLogic.UpdateServiceRequestStatus(expectedServiceRequest.Id, maintenancePerson.Id, null);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }

        [TestMethod]
        public void UpdateServiceRequestToRejectedCorrectTestLogic()
        {
            //Arrange
            User maintenancePerson = new User()
            {
                Id = Guid.NewGuid(),
                Role = Roles.Maintenance
            };

            Category category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Category 1",
            };

            Apartment apartment = new Apartment()
            {
                Id = Guid.NewGuid(),
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" }
            };

            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = category,
                Apartment = apartment,
                Status = ServiceRequestStatus.Attending,
                MaintenancePerson = maintenancePerson
            };

            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = serviceRequest.Id,
                Description = serviceRequest.Description,
                Category = serviceRequest.Category,
                Apartment = serviceRequest.Apartment,
                Status = ServiceRequestStatus.Closed,
                TotalCost = 100,
                MaintenancePerson = maintenancePerson
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.UpdateServiceRequest(It.IsAny<ServiceRequest>()));
            serviceRequestRepo.Setup(l => l.GetServiceRequestById(It.IsAny<Guid>())).Returns(serviceRequest);

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            //Act
            ServiceRequest logicResult = requestLogic.UpdateServiceRequestStatus(expectedServiceRequest.Id, maintenancePerson.Id, 100);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }

        [TestMethod]
     
        public void CantCloseRequestThatIsNotAttendingTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);
            try
            {
                User maitenancePerson = new User()
                {
                    Id = Guid.NewGuid(),
                    Role = Roles.Maintenance
                };

                Category category = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Category 1",
                };

                Apartment apartment = new Apartment()
                {
                    Id = Guid.NewGuid(),
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" }
                };

                ServiceRequest serviceRequest = new ServiceRequest()
                {
                    Id = Guid.NewGuid(),
                    Description = "Service Request 1",
                    Category = category,
                    Apartment = apartment,
                    Status = ServiceRequestStatus.Open,
                    MaintenancePerson = maitenancePerson
                };

                ServiceRequest expectedServiceRequest = new ServiceRequest()
                {
                    Id = serviceRequest.Id,
                    Description = serviceRequest.Description,
                    Category = serviceRequest.Category,
                    Apartment = serviceRequest.Apartment,
                    Status = ServiceRequestStatus.Closed,
                    TotalCost = 100,
                    MaintenancePerson = maitenancePerson
                };

                serviceRequestRepo.Setup(l => l.GetServiceRequestById(It.IsAny<Guid>())).Returns(serviceRequest);

                // Act
                ServiceRequest logicResult = requestLogic.UpdateServiceRequestStatus(expectedServiceRequest.Id, maitenancePerson.Id, 100);

            }
            catch (InvalidOperationException e)
            {
                specificEx = e;
            }

            // Assert
            serviceRequestRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(InvalidOperationException));
            Assert.IsTrue(specificEx.Message.Contains("Service request is not attending"));
        }

        [TestMethod]
        public void GetAllServiceRequestsMaintenanceCorrectTestLogic()
        {
            // Arrange
            User maintenancePerson = new User()
            {
                Id = Guid.NewGuid(),
                Role = Roles.Maintenance
            };

            Category category = new Category { Name = "Category 1" };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "mail@gmail.com" },
                Rooms = 3,
                Bathrooms = 2,
                HasTerrace = true
            };

            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "A description",
                Apartment = apartment,
                Category = category,
                Status = ServiceRequestStatus.Attending,
                MaintenancePerson = maintenancePerson
            };

            IEnumerable<ServiceRequest> expectedServiceRequests = new List<ServiceRequest>
            {
                serviceRequest
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<ICategoryRepository> categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(repo => repo.GetAllServiceRequestsByMaintenanceUserId(It.IsAny<Guid>())).Returns(expectedServiceRequests);

            RequestLogic serviceRequestController = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object, userRepository.Object);

            // Act
            IEnumerable<ServiceRequest> logicResult = serviceRequestController.GetAllServiceRequestsMaintenance(maintenancePerson.Id);

            // Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequests);
        }
    }
}
