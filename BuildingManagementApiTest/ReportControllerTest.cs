using BuildingManagementApi.Controllers;
using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.Out;
using ModelsApi.In;
using Moq;
using Domain.@enum;
using Microsoft.AspNetCore.Http;

namespace BuildingManagementApiTest;

[TestClass]
public class ReportControllerTest
{
    [TestMethod]
    public void GetReportCorrectControllerTest()
    {
        // Arrange
        Guid managerId = Guid.NewGuid();
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e"; // Simula un token de autenticación válido

        IEnumerable<(string, int, int, int)> reportInfo = new List<(string, int, int, int)>
        {
            ("BuildingName", 3, 2, 1)
        };

        IEnumerable<ReportResponse> expectedResponse = new List<ReportResponse>
        {
         new ReportResponse(reportInfo.First())
        };

       
        var reportLogic = new Mock<IReportLogic>();
        var sessionLogic = new Mock<ISessionService>();

        reportLogic.Setup(p => p.GetReport(It.IsAny<Guid>(), It.IsAny<string>())).Returns(reportInfo);
        sessionLogic.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(new User { Id = managerId, Role = Roles.Manager });

        var controller = new ReportController(reportLogic.Object, sessionLogic.Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        // Act
        var result = controller.GetReport("") as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        var value = result.Value as IEnumerable<ReportResponse>;
        Assert.IsNotNull(value);


        // Assert
        Assert.IsNotNull(value);
        Assert.AreEqual(expectedResponse.First().Name, value.First().Name);
        Assert.AreEqual(expectedResponse.First().PendingRequests, value.First().PendingRequests);
        Assert.AreEqual(expectedResponse.First().InProgressRequests, value.First().InProgressRequests);
        Assert.AreEqual(expectedResponse.First().DoneRequests, value.First().DoneRequests);
    }

    [TestMethod]
    public void GetMaintenanceReportCorrectControllerTest()
    {
        // Arrange
        string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";

        IEnumerable<(string, int, int, int, string)> reportInfo = new List<(string, int, int, int, string)>
        {
            ("BuildingName", 3, 2, 1, "5hs")
        };
        
        IEnumerable<MaintenanceReportResponse> expectedResponse = new List<MaintenanceReportResponse>
        {
            new MaintenanceReportResponse(reportInfo.First())
        };

        var reportLogic = new Mock<IReportLogic>();
        var sessionLogic = new Mock<ISessionService>();

        reportLogic.Setup(p => p.GetMaintenanceReport(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid?>())).Returns(reportInfo);
        sessionLogic.Setup(p => p.GetUserByToken(It.IsAny<Guid>())).Returns(new User { Id = Guid.NewGuid(), Role = Roles.Manager });

        var controller = new ReportController(reportLogic.Object, sessionLogic.Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

        // Act
        var result = controller.GetMaintenanceReport("BuildingName", null) as OkObjectResult;

        var value = result.Value as IEnumerable<MaintenanceReportResponse>;
        // Assert
        reportLogic.VerifyAll();
        sessionLogic.VerifyAll();
        Assert.IsNotNull(result);
        Assert.IsNotNull(value);
        Assert.AreEqual(expectedResponse.First(), value.First());

    }

}
