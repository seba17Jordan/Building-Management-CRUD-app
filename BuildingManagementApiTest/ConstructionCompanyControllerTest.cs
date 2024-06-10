using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LogicInterface;
using ModelsApi.In;
using ModelsApi.Out;
using BuildingManagementApi.Controllers;
using Domain;
using Domain.@enum;
using Microsoft.AspNetCore.Http;

namespace BuildingManagementApi.Tests.Controllers
{
    [TestClass]
    public class ConstructionCompanyControllerTest
    {
        private Mock<IConstructionCompanyLogic> _constructionCompanyLogic;
        private ISessionService _sessionService;
        private ConstructionCompanyController _controller;

        [TestInitialize]
        public void Setup()
        {
            _constructionCompanyLogic = new Mock<IConstructionCompanyLogic>(MockBehavior.Strict);
            _controller = new ConstructionCompanyController(_constructionCompanyLogic.Object, _sessionService);
        }

        [TestMethod]
        public void CreateConstructionCompanyTest()
        {
            // Arrange
            var validToken = Guid.Parse("b4d9e6a4-466c-4a4f-91ea-6d7e7997584e");
            var constructionCompanyRequest = new ConstructionCompanyRequest
            {
                Name = "Test"
            };

            // Mock del usuario administrador
            var constructionCompanyAdmin = new User
            {
                Id = Guid.NewGuid(),
                Email = "lkand@gmail.com",
                Password = "1234",
                Role = Roles.ConstructionCompanyAdmin,
                Name = "Lukas",
                LastName = "Kand"
            };

            // Configurar el mock para el servicio de sesión
            var _sessionServiceMock = new Mock<ISessionService>();
            _sessionServiceMock.Setup(x => x.GetUserByToken(validToken)).Returns(constructionCompanyAdmin);

            // Configurar el mock para la lógica de la compañía de construcción
            var _constructionCompanyLogicMock = new Mock<IConstructionCompanyLogic>();
            _constructionCompanyLogicMock.Setup(x => x.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(new ConstructionCompany());

            // Crear una instancia del controlador pasando los mocks como dependencias
            var _controller = new ConstructionCompanyController(_constructionCompanyLogicMock.Object, _sessionServiceMock.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = validToken.ToString();

            // Act
            var result = _controller.CreateConstructionCompany(constructionCompanyRequest) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual("CreateConstructionCompany", result.ActionName);
        }

        [TestMethod]
        public void UpdateConstructionCompanyNameControllerTest()
        {
            //Arrange
            string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
            ConstructionCompanyRequest constructionCompanyRequest = new ConstructionCompanyRequest
            {
                Name = "NewName"
            };

            ConstructionCompany constructionCompany = new ConstructionCompany
            {
                Id = Guid.NewGuid(),
                Name = "OldName"
            };

            ConstructionCompany updatedConstructionCompany = new ConstructionCompany
            {
                Id = constructionCompany.Id,
                Name = constructionCompanyRequest.Name
            };

            User constructionCompanyAdmin = new User
            {
                Id = Guid.NewGuid(),
                Email = "aeda@gmail.com",
                Password = "1234",
                Role = Roles.ConstructionCompanyAdmin,
                Name = "Aeda",
                LastName = "Kand"
            };

            ConstructionCompanyResponse expectedMappedCompany = new ConstructionCompanyResponse(updatedConstructionCompany);

            Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);
            Mock<IConstructionCompanyLogic> constructionCompanyLogic = new Mock<IConstructionCompanyLogic>(MockBehavior.Strict);

            sessionService.Setup(x => x.GetUserByToken(It.IsAny<Guid>())).Returns(constructionCompanyAdmin);
            constructionCompanyLogic.Setup(x => x.UpdateConstructionCompanyName(It.IsAny<string>(), It.IsAny<User>())).Returns(updatedConstructionCompany);

            ConstructionCompanyController constructionCompanyController = new ConstructionCompanyController(constructionCompanyLogic.Object, sessionService.Object);
            constructionCompanyController.ControllerContext.HttpContext = new DefaultHttpContext();
            constructionCompanyController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

            OkObjectResult expectedObjResult = new OkObjectResult(expectedMappedCompany);

            //Act
            var controllerResult = constructionCompanyController.UpdateConstructionCompanyName(constructionCompanyRequest);

            //Assert
            sessionService.VerifyAll();
            constructionCompanyLogic.VerifyAll();

            OkObjectResult resultObject = controllerResult as OkObjectResult;
            ConstructionCompanyResponse resultValue = resultObject.Value as ConstructionCompanyResponse;

            Assert.AreEqual(resultObject.StatusCode, expectedObjResult.StatusCode);
            Assert.AreEqual(resultValue, expectedMappedCompany);
        }

        [TestMethod]
        public void GetConstructionCompanyControllerTest()
        {
            //Arrange
            string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";
            ConstructionCompanyRequest constructionCompanyRequest = new ConstructionCompanyRequest
            {
                Name = "NewName"
            };

            ConstructionCompany constructionCompany = new ConstructionCompany
            {
                Id = Guid.NewGuid(),
                Name = "OldName"
            };

            User constructionCompanyAdmin = new User
            {
                Id = Guid.NewGuid(),
                Email = "aeda@gmail.com",
                Password = "1234",
                Role = Roles.ConstructionCompanyAdmin,
                Name = "Aeda",
                LastName = "Kand"
            };

            ConstructionCompanyResponse expectedMappedCompany = new ConstructionCompanyResponse(constructionCompany);

            Mock<ISessionService> sessionService = new Mock<ISessionService>(MockBehavior.Strict);
            Mock<IConstructionCompanyLogic> constructionCompanyLogic = new Mock<IConstructionCompanyLogic>(MockBehavior.Strict);

            sessionService.Setup(x => x.GetUserByToken(It.IsAny<Guid>())).Returns(constructionCompanyAdmin);
            constructionCompanyLogic.Setup(x => x.GetConstructionCompany(It.IsAny<User>())).Returns(constructionCompany);

            ConstructionCompanyController constructionCompanyController = new ConstructionCompanyController(constructionCompanyLogic.Object, sessionService.Object);
            constructionCompanyController.ControllerContext.HttpContext = new DefaultHttpContext();
            constructionCompanyController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

            OkObjectResult expectedObjResult = new OkObjectResult(expectedMappedCompany);

            //Act
            var controllerResult = constructionCompanyController.GetConstructionCompany();

            //Assert
            sessionService.VerifyAll();
            constructionCompanyLogic.VerifyAll();

            OkObjectResult resultObject = controllerResult as OkObjectResult;
            ConstructionCompanyResponse resultValue = resultObject.Value as ConstructionCompanyResponse;

            Assert.AreEqual(resultObject.StatusCode, expectedObjResult.StatusCode);
            Assert.AreEqual(resultValue, expectedMappedCompany);
        }
    }
}
