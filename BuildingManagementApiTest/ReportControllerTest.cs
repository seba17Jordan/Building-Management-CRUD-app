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
    public void GetReportCorrectTest()
    {
        // Arrange
        Guid managerId = Guid.NewGuid();
        IEnumerable<(string, int, int, int)> reportInfo = new List<(string, int, int, int)>
        {
        ("BuildingName", 3, 2, 1)
        };
        IEnumerable<ReportResponse> expectedResponse = new List<ReportResponse>
        {
        new ReportResponse(reportInfo.First())
        };

        // Mock del IReportLogic
        var mockLogic = new Mock<IReportLogic>();
        mockLogic.Setup(p => p.GetReport(managerId, "param")).Returns(reportInfo);

        // Crear el controlador y configurar el contexto
        var controller = new ReportController(mockLogic.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.HttpContext.Items["UserId"] = managerId.ToString();

        // Act
        var result = controller.GetReport("param") as OkObjectResult;
        var value = result.Value as IEnumerable<ReportResponse>;

        // Assert
        Assert.IsNotNull(value);
        Assert.AreEqual(expectedResponse.First().Name, value.First().Name);
        Assert.AreEqual(expectedResponse.First().PendingRequests, value.First().PendingRequests);
        Assert.AreEqual(expectedResponse.First().InProgressRequests, value.First().InProgressRequests);
        Assert.AreEqual(expectedResponse.First().DoneRequests, value.First().DoneRequests);
    }

}
