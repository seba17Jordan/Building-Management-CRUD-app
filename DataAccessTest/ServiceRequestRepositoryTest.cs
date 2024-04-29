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
            // Arrange
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = new Category { Name = "Category 1" },
                Apartment = new Apartment()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                    Rooms = 3,
                    Bathrooms = 2,
                    HasTerrace = true
                }
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
            // Arrange
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Status = ServiceRequestStatus.Open,
                Category = new Category { Name = "Category 1" },
                Apartment = new Apartment()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                    Rooms = 3,
                    Bathrooms = 2,
                    HasTerrace = true
                }
            };

            var context = CreateDbContext("ServiceRequestExistsTestDataAccess");
            var serviceRequestRepo = new ServiceRequestRepository(context);

            context.Set<ServiceRequest>().Add(expectedServiceRequest);
            context.SaveChanges();

            // Act
            bool exists = serviceRequestRepo.ServiceRequestExists(expectedServiceRequest.Id);
            Assert.IsTrue(exists);

        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}