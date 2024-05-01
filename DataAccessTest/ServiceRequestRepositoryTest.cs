using DataAccess;
using Domain;
using Domain.@enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessTest
{
    [TestClass]
    public class ServiceRequestRepositoryTest
    {
        [TestMethod]
        public void CreateServiceRequestCorrectTestDataAccess()
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

            // Arrange
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                Apartment = apartment.Id,
                CategoryName = category.Name
            };

            var context = CreateDbContext("CreateServiceRequestCorrectTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            // Act
            ServiceRequest createdServiceRequest = serviceRequestRepo.CreateServiceRequest(expectedServiceRequest);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedServiceRequest, createdServiceRequest);
        }

        [TestMethod]
        public void ServiceRequestExistsTestDataAccess()
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

            // Arrange
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                Apartment = apartment.Id,
                CategoryName = category.Name
            };

            var context = CreateDbContext("ServiceRequestExistsTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(expectedServiceRequest);
            context.SaveChanges();

            // Act
            bool exists = serviceRequestRepo.ServiceRequestExists(expectedServiceRequest.Id);
            Assert.IsTrue(exists);

        }

        [TestMethod]
        public void GetServiceRequestByIdTestDataAccess()
        {
            Category category = new Category { Name = "Category 1" };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "email@gmail.com" },
                Rooms = 3,
                Bathrooms = 2,
                HasTerrace = true
            };

            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                Apartment = apartment.Id,
                CategoryName = category.Name
            };

            var context = CreateDbContext("GetServiceRequestByIdTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(expectedServiceRequest);
            context.SaveChanges();

            // Act
            ServiceRequest serviceRequest = serviceRequestRepo.GetServiceRequestById(expectedServiceRequest.Id);

            // Assert
            Assert.AreEqual(expectedServiceRequest, serviceRequest);
        }

        [TestMethod]
        public void UpdateServiceRequestTestDataAccess()
        {
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
            
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                Apartment = apartment.Id,
                CategoryName = category.Name
            };

            var context = CreateDbContext("UpdateServiceRequestTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(expectedServiceRequest);
            context.SaveChanges();

            expectedServiceRequest.MaintainancePersonId = Guid.NewGuid();
            expectedServiceRequest.Status = ServiceRequestStatus.Attending;

            // Act
            serviceRequestRepo.UpdateServiceRequest(expectedServiceRequest);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(ServiceRequestStatus.Attending, context.Set<ServiceRequest>().Find(expectedServiceRequest.Id).Status);

        }

        [TestMethod]
        public void GetAllServiceRequestsByManagerTestDataAccess()
        {
            Category category = new Category { Name = "Category 1" };

            User manager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "alekd@gmiail.com",
                Role = Roles.Manager
            };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "alonso@gmail.com" },
                Rooms = 3,
                Bathrooms = 2,
                HasTerrace = true
            };

            ServiceRequest serviceRequest1 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                ManagerId = manager.Id
            };

            ServiceRequest serviceRequest2 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                ManagerId = manager.Id
            };

            var context = CreateDbContext("GetAllServiceRequestsTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(serviceRequest1);
            context.Set<ServiceRequest>().Add(serviceRequest2);
            context.SaveChanges();

            // Act
            IEnumerable<ServiceRequest> serviceRequests = serviceRequestRepo.GetAllServiceRequestsManager("", manager.Id);
            List<ServiceRequest> serviceRequestsList = serviceRequests.ToList();

            // Assert
            Assert.AreEqual(2, serviceRequestsList.Count);
            Assert.AreEqual(serviceRequest1, serviceRequestsList[0]);
            Assert.AreEqual(serviceRequest2, serviceRequestsList[1]);
        }

        [TestMethod]
        public void GetAllServiceRequestsFilterByNameTestDataAccess()
        {
            Category category1 = new Category { Name = "Category 1" };
            Category category2 = new Category { Name = "Category 2" };

            User manager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "safae@gmail.com",
                Role = Roles.Manager
            };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" },
                Rooms = 3,
                Bathrooms = 2,
                HasTerrace = true
            };

            ServiceRequest serviceRequest1 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category1.Id,
                CategoryName = category1.Name,
                Apartment = apartment.Id,
                ManagerId = manager.Id
            };

            ServiceRequest serviceRequest2 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Status = ServiceRequestStatus.Open,
                Category = category2.Id,
                CategoryName = category2.Name,
                Apartment = apartment.Id,
                ManagerId = manager.Id
            };

            var context = CreateDbContext("GetAllServiceRequestsFilterByNameTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(serviceRequest1);
            context.Set<ServiceRequest>().Add(serviceRequest2);
            context.SaveChanges();

            // Act
            IEnumerable<ServiceRequest> serviceRequests = serviceRequestRepo.GetAllServiceRequestsManager("Category 1", manager.Id);
            List<ServiceRequest> serviceRequestsList = serviceRequests.ToList();

            // Assert
            Assert.AreEqual(1, serviceRequestsList.Count);
            Assert.AreEqual(serviceRequest1, serviceRequestsList[0]);
        }

        [TestMethod]
        public void GetAllServiceRequestsByMaintenanceUserIdTestDataAccess()
        {

            User maintenancePerson1 = new User()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "xs@gmail.com",
                Role = Roles.Maintenance
            };

            User maintenancePerson2 = new User()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "aeda@gmail.com",
                Role = Roles.Maintenance
            };

            Category category = new Category { Name = "Category 1" };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "xd@gmial.com" },
                Rooms = 3,
                Bathrooms = 2,
                HasTerrace = true
            };

            ServiceRequest serviceRequest1 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                MaintainancePersonId = maintenancePerson1.Id
            };

            ServiceRequest serviceRequest2 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                MaintainancePersonId  = maintenancePerson2.Id
            };

            var context = CreateDbContext("GetAllServiceRequestsByMaintenanceUserIdTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(serviceRequest1);
            context.Set<ServiceRequest>().Add(serviceRequest2);
            context.SaveChanges();

            // Act
            IEnumerable<ServiceRequest> serviceRequests = serviceRequestRepo.GetAllServiceRequestsByMaintenanceUserId(maintenancePerson1.Id);
            List<ServiceRequest> serviceRequestsList = serviceRequests.ToList();

            // Assert
            Assert.AreEqual(1, serviceRequestsList.Count);
            Assert.AreEqual(serviceRequest1, serviceRequestsList[0]);
        }

        [TestMethod]
        public void GetAllServiceRequestsGeneralTestDataAccess()
        {
            User maintenancePerson = new User()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "daed@gmail.com"
            };

            Category category = new Category { Name = "Category 1" };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "sa@gmail.com" },
                Rooms = 3,
                Bathrooms = 2,
                HasTerrace = true
            };

            ServiceRequest serviceRequest1 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                MaintainancePersonId = maintenancePerson.Id
            };

            ServiceRequest serviceRequest2 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                MaintainancePersonId = maintenancePerson.Id
            };

            var context = CreateDbContext("GetAllServiceRequestsGeneralTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(serviceRequest1);
            context.Set<ServiceRequest>().Add(serviceRequest2);
            context.SaveChanges();

            // Act
            IEnumerable<ServiceRequest> serviceRequests = serviceRequestRepo.GetAllServiceRequests();
            List<ServiceRequest> serviceRequestsList = serviceRequests.ToList();

            // Assert
            Assert.AreEqual(2, serviceRequestsList.Count);
            Assert.AreEqual(serviceRequest1, serviceRequestsList[0]);
            Assert.AreEqual(serviceRequest2, serviceRequestsList[1]);
        }

        [TestMethod]
        public void GetServiceReqByBuildingTestDataAccess()
        {
            Category category = new Category { Name = "Category 1" };

            Apartment apartment = new Apartment()
            {
                Floor = 1,
                Number = 101,
                Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "" }
            };

            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Address = "Address 1",
                Name = "Building 1",
                Apartments = new List<Apartment> { apartment }
            };

            ServiceRequest serviceRequest1 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                BuildingId = building.Id
            };

            ServiceRequest serviceRequest2 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id,
                BuildingId = building.Id
            };

            var context = CreateDbContext("GetServiceReqByBuildingTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(serviceRequest1);
            context.Set<ServiceRequest>().Add(serviceRequest2);
            context.SaveChanges();

            // Act
            List<ServiceRequest> serviceRequests = serviceRequestRepo.GetServiceRequestsByBuilding(building.Id);
            Assert.AreEqual(2, serviceRequests.Count);
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}