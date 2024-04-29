using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
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
            serviceRequestRepo.Setup(l => l.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns(expectedServiceRequest);
            buildingRepo.Setup(l => l.ExistApartment(It.IsAny<Guid>())).Returns(true);
            categoryRepo.Setup(l => l.FindCategoryById(It.IsAny<Guid>())).Returns(true);

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object,buildingRepo.Object, categoryRepo.Object);

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
            serviceRequestRepo.Setup(l => l.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns(expectedServiceRequest);

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object, buildingRepo.Object, categoryRepo.Object);

            //Act
            ServiceRequest logicResult = requestLogic.CreateServiceRequest(expectedServiceRequest);
            serviceRequestRepo.VerifyAll();
        }

    }
}
