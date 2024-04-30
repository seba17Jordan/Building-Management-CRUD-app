using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
using Domain.@enum;
namespace BusinessLogicTest
{
    [TestClass]
    public class ReportsLogicTest
    {
        [TestMethod]
        public void CreateReportBuildingTestCorrect()
        {
            // Arrange

            Building building = new Building
            {
                Id = Guid.NewGuid(),
                Name = "BuildingName"
            };

            Guid managerId = Guid.NewGuid();
            IEnumerable<(string, int, int, int)> reportInfo = new List<(string, int, int, int)>
            {
                ("BuildingName", 1, 1, 1)
            };

            IEnumerable<(string, int, int, int)> expectedResponse = new List<(string, int, int, int)>
            {
                reportInfo.First()
            };

            var UserRepository = new Mock<IUserRepository>();
            var BuildingRepository = new Mock<IBuildingRepository>();
            var ServiceRequestRepository = new Mock<IServiceRequestRepository>();

            BuildingRepository.Setup(p => p.GetAllBuildings(managerId)).Returns(new List<Building>
            {
                building
            });

            ServiceRequestRepository.Setup(p => p.GetAllServiceRequests("")).Returns(new List<ServiceRequest>
            {
                new ServiceRequest
                {
                    Id = Guid.NewGuid(),
                    BuildingId = BuildingRepository.Object.GetAllBuildings(managerId).First().Id,
                    Status = ServiceRequestStatus.Open
                },
                new ServiceRequest
                {
                    Id = Guid.NewGuid(),
                    BuildingId = BuildingRepository.Object.GetAllBuildings(managerId).First().Id,
                    Status = ServiceRequestStatus.Attending
                },
                new ServiceRequest
                {
                    Id = Guid.NewGuid(),
                    BuildingId = BuildingRepository.Object.GetAllBuildings(managerId).First().Id,
                    Status = ServiceRequestStatus.Closed
                }
            });

            ReportLogic reportLogic = new ReportLogic(ServiceRequestRepository.Object, BuildingRepository.Object, UserRepository.Object);

            // Act
            var result = reportLogic.GetReport(managerId, "BuildingName");
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.First().Item1, result.First().Item1);
            Assert.AreEqual(expectedResponse.First().Item2, result.First().Item2);
            Assert.AreEqual(expectedResponse.First().Item3, result.First().Item3);
            Assert.AreEqual(expectedResponse.First().Item4, result.First().Item4);

        }
    }
}
