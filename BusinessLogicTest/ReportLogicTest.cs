using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
using Domain.@enum;
namespace BusinessLogicTest
{
    [TestClass]
    public class ReportLogicTest
    {
        [TestMethod]
        public void CreateReportLogicTest()
        {
            // Arrange
            var expectedReport = new List<(string, int, int, int)>
            {
                ("Building1", 3, 0, 0)
            };

            var serviceRequestRepository = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            serviceRequestRepository.Setup(l => l.GetAllRequests()).Returns(new List<ServiceRequest>
            {
                new ServiceRequest { BuildingId = Guid.NewGuid(), Status = ServiceRequestStatus.Open },
                new ServiceRequest { BuildingId = Guid.NewGuid(), Status = ServiceRequestStatus.Open },
                new ServiceRequest { BuildingId = Guid.NewGuid(), Status = ServiceRequestStatus.Open }
            });

            var buildingRepository = new Mock<IBuildingRepository>(MockBehavior.Strict);
            buildingRepository.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(new Building { Name = "Building1" });

            var reportLogic = new ReportLogic(serviceRequestRepository.Object, buildingRepository.Object);

            // Act
            var result = reportLogic.GetReport(Guid.NewGuid(), "building");

            // Assert
            serviceRequestRepository.VerifyAll();
            buildingRepository.VerifyAll();

            Assert.AreEqual(expectedReport.Count(), result.Count());
            for (int i = 0; i < expectedReport.Count(); i++)
            {
                Assert.AreEqual(expectedReport[i].Item1, result.ElementAt(i).Item1);
                Assert.AreEqual(expectedReport[i].Item2, result.ElementAt(i).Item2);
                Assert.AreEqual(expectedReport[i].Item3, result.ElementAt(i).Item3);
                Assert.AreEqual(expectedReport[i].Item4, result.ElementAt(i).Item4);
            }
        }
    }
}