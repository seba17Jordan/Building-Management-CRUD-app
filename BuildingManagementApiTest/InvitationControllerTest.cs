using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LogicInterface;
using BuildingManagementApi.Controllers;
using ModelsApi.In;
using ModelsApi.Out;
using Domain;
using System;
using Domain.@enum;

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

            var responseValue = createdResponse.Value as InvitationResponse;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.Equals(responseValue));
        }

        [TestMethod]
        public void UpdateInvitationState_WithValidData()
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

            var request = new UpdateInvitationStateRequest
            {
                Status = Status.Accepted
            };

            var updatedInvitation = new Invitation
            {
                Id = invitationId,
                Email = invitation.Email,
                Name = invitation.Name,
                ExpirationDate = invitation.ExpirationDate,
                State = request.Status
            };

            _invitationLogicMock.Setup(l => l.UpdateInvitationState(invitationId, request.Status)).Returns(updatedInvitation);
            // Act
            IActionResult actionResult = _controller.UpdateInvitationState(invitationId, request);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var responseValue = okResult.Value as InvitationResponse;
            Assert.IsNotNull(responseValue);
            Assert.AreEqual(invitationId, responseValue.Id);
            Assert.AreEqual(request.Status, responseValue.State);
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

        [TestMethod]
        public void DeleteCorrectInvitation()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var invitationToDelete = new Invitation
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Name = "name",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _invitationLogicMock.Setup(l => l.DeleteInvitation(invitationId));

            // Act
            IActionResult deleteActionResult = _controller.DeleteInvitation(invitationId);
            var noContentResult = deleteActionResult as NoContentResult;

            // Assert
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
            _invitationLogicMock.Verify(l => l.DeleteInvitation(invitationId), Times.Once);
        }

    }
}
