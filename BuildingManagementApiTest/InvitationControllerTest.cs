using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LogicInterface;
using BuildingManagementApi.Controllers;
using ModelsApi.In;
using ModelsApi.Out;
using Domain;
using System;

namespace BuildingManagementApiTests.Controllers
{
    [TestClass]
    public class InvitationControllerTests
    {
        private Mock<IInvitationLogic> _invitationLogicMock;
        private InvitationController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _invitationLogicMock = new Mock<IInvitationLogic>();
            _controller = new InvitationController(_invitationLogicMock.Object);

        }

        [TestMethod]
        public void CreateInvitation_WithValidData_ShouldReturnCreatedResponse()
        {
            // Arrange
            var request = new CreateInvitationRequest
            {
                Email = "test@example.com",
                Name = "Test Name",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            var createdInvitation = new Invitation
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Name = request.Name,
                ExpirationDate = request.ExpirationDate
            };

            _invitationLogicMock.Setup(l => l.CreateInvitation(It.IsAny<Invitation>())).Returns(createdInvitation);

            // Act
            IActionResult actionResult = _controller.CreateInvitation(request);
            var createdResponse = actionResult as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(createdResponse);
            Assert.AreEqual(201, createdResponse.StatusCode);

            var responseValue = createdResponse.Value as CreateInvitationResponse;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.Equals(responseValue));
        }

        [TestMethod]
        public void AcceptInvitation_WithValidData_ShouldReturnOkManager()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new AcceptInvitationRequest
            {
                Email = "test@example.com",
                Password = "password"
            };

            var manager = new Manager
            {
                Email = request.Email,
                Name = "Test Manager",
                Password = request.Password
            };

            _invitationLogicMock.Setup(l => l.AcceptInvitation(id, request.Email, request.Password)).Returns(manager);

            // Act
            IActionResult actionResult = _controller.AcceptInvitation(id, request);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var responseValue = okResult.Value as AcceptInvitationResponse;
            Assert.IsNotNull(responseValue);
            Assert.AreEqual(manager.Email, responseValue.Email);
            Assert.AreEqual(manager.Name, responseValue.Name);
            Assert.AreEqual(manager.Password, responseValue.Password);   
        }

        [TestMethod]
        public void RejectCorrectInvitation()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var invitation = new Invitation
            {
                Id = invitationId,
                Email = "test@example.com",
                Name = "name",
                ExpirationDate = DateTime.Now
            };

            _invitationLogicMock.Setup(l => l.RejectInvitation(invitation.Id));
            var expectedObjectResult = new NoContentResult();

            // Act
            var result = _controller.RejectInvitation(invitation.Id);

            // Assert
            _invitationLogicMock.Verify(l => l.RejectInvitation(invitation.Id), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var resultObject = result as NoContentResult;
            Assert.AreEqual(expectedObjectResult.StatusCode, resultObject.StatusCode);
        }
    }
}
