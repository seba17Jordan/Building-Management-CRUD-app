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
            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building1"
            };
            Building building2 = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building2"
            };
            Building building3 = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building3"
            };
            // Arrange
            var expectedReport = new List<(string, int, int, int)>
            {
                ("Building1", 3, 0, 0)
            };

            var serviceRequestRepository = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            serviceRequestRepository.Setup(l => l.GetAllServiceRequests("")).Returns(new List<ServiceRequest>
            {
                new ServiceRequest { BuildingId = building.Id, Status = ServiceRequestStatus.Open },
                new ServiceRequest { BuildingId = building.Id, Status = ServiceRequestStatus.Open },
                new ServiceRequest { BuildingId = building.Id, Status = ServiceRequestStatus.Open }
            });

            var buildingRepository = new Mock<IBuildingRepository>(MockBehavior.Strict);
            buildingRepository.Setup(l => l.GetAllBuildings(It.IsAny<Guid>())).Returns(new List<Building>
            {
                building,building2,building3
            });

            var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepository.Setup(l => l.GetUserById(It.IsAny<Guid>())).Returns(new User { Id = Guid.NewGuid(), Role = Roles.Manager });

            var reportLogic = new ReportLogic(serviceRequestRepository.Object, buildingRepository.Object,userRepository.Object);

            // Act
            var result = reportLogic.GetReport(Guid.NewGuid(), "Building1");

            // Assert
            serviceRequestRepository.VerifyAll();
            buildingRepository.VerifyAll();

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