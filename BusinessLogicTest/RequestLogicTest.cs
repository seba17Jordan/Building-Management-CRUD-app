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
            //Arrange
            ServiceRequest expectedServiceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                Description = "Service Request 1",
                Category = new Category { Name = "Category 1" },
                Apartment = new Apartment { Floor = 1, Number = 101 }
            };

            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            serviceRequestRepo.Setup(l => l.CreateServiceRequest(It.IsAny<ServiceRequest>())).Returns(expectedServiceRequest);

            RequestLogic requestLogic = new RequestLogic(serviceRequestRepo.Object);

            //Act
            ServiceRequest logicResult = requestLogic.CreateServiceRequest(expectedServiceRequest);

            //Assert
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedServiceRequest);
        }
    }
}
