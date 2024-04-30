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
        public void GetAllServiceRequestsTestDataAccess()
        {
            Category category = new Category { Name = "Category 1" };

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
                Apartment = apartment.Id
            };

            ServiceRequest serviceRequest2 = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Status = ServiceRequestStatus.Open,
                Category = category.Id,
                CategoryName = category.Name,
                Apartment = apartment.Id
            };

            var context = CreateDbContext("GetAllServiceRequestsTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(serviceRequest1);
            context.Set<ServiceRequest>().Add(serviceRequest2);
            context.SaveChanges();

            // Act
            IEnumerable<ServiceRequest> serviceRequests = serviceRequestRepo.GetAllServiceRequests("");
            List<ServiceRequest> serviceRequestsList = serviceRequests.ToList();

            // Assert
            Assert.AreEqual(2, serviceRequestsList.Count);
            Assert.AreEqual(serviceRequest1, serviceRequestsList[0]);
            Assert.AreEqual(serviceRequest2, serviceRequestsList[1]);
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}