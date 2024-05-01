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
            User manager = new User
            {
                Id = Guid.NewGuid(),
                Role = Roles.Manager
            };

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

            ServiceRequestRepository.Setup(p => p.GetAllServiceRequests()).Returns(new List<ServiceRequest>
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

        [TestMethod]
        public void GetMaintenanceReportCorrectTestLogic()
        {
            User manager = new User
            {
                Id = Guid.NewGuid(),
                Role = Roles.Manager
            };

            User maintenancePerson = new User
            {
                Role = Roles.Maintenance,
                Id = Guid.NewGuid(),
                Name = "Mantenimiento"
            };

            Apartment apartment = new Apartment
            {
                Id = Guid.NewGuid()
            };

            Building building = new Building
            {
                Id = Guid.NewGuid(),
                Name = "BuildingName",
                Apartments = new List<Apartment>
                {
                    apartment
                }
            };

            Category category = new Category { Name = "CategoryName" };

            ServiceRequest serviceRequest = new ServiceRequest
            {
                Id = Guid.NewGuid(),
                BuildingId = building.Id,
                Apartment = apartment.Id,
                Status = ServiceRequestStatus.Closed,
                Category = category.Id,
                MaintainancePersonId = maintenancePerson.Id,
                ManagerId = manager.Id,
                StartDate = new DateTime(2022, 4, 25, 10, 0, 0),
                EndDate = new DateTime(2022, 4, 25, 15, 0, 0)
        };

            IEnumerable<(string, int, int, int, string)> expectedResponse = new List<(string, int, int, int, string)>
            {
                 ("Mantenimiento", 0, 0, 1, "5hs")
            };

            var UserRepository = new Mock<IUserRepository>();
            var BuildingRepository = new Mock<IBuildingRepository>();
            var ServiceRequestRepository = new Mock<IServiceRequestRepository>();

            BuildingRepository.Setup(p => p.GetBuildingByName(It.IsAny<string>())).Returns(building);
            UserRepository.Setup(p => p.GetUserByName(It.IsAny<string>())).Returns(maintenancePerson);
            ServiceRequestRepository.Setup(p => p.GetServiceRequestsByBuilding(It.IsAny<Guid>())).Returns(new List<ServiceRequest>
            {
                serviceRequest
            });
            UserRepository.Setup(p => p.GetUserById(It.IsAny<Guid>())).Returns(maintenancePerson);


            ReportLogic reportLogic = new ReportLogic(ServiceRequestRepository.Object, BuildingRepository.Object, UserRepository.Object);

            // Act
            var result = reportLogic.GetMaintenanceReport("BuildingName", manager.Id, "dac");

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.First().Item1, result.First().Item1);
            Assert.AreEqual(expectedResponse.First().Item2, result.First().Item2);
            Assert.AreEqual(expectedResponse.First().Item3, result.First().Item3);
            Assert.AreEqual(expectedResponse.First().Item4, result.First().Item4);
            Assert.AreEqual(expectedResponse.First().Item5, result.First().Item5);
        }
    }
}
