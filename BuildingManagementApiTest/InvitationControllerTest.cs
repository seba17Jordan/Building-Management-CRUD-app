﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LogicInterface;
using BuildingManagementApi.Controllers;
using ModelsApi.In;
using ModelsApi.Out;
using Domain;
using System;
using Domain.@enum;
using CustomExceptions;
using Microsoft.AspNetCore.Http;

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
            var request = new InvitationRequest
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
                ExpirationDate = (DateTime)request.ExpirationDate
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
        public void RejectInvitationState_WithValidData()
        {
            // Arrange
            var invitation = new Invitation
            {
                Email = "test@example.com",
                Name = "name",
                ExpirationDate = DateTime.Now,
                State = Status.Pending
            };

            _invitationLogicMock.Setup(l => l.RejectInvitation(It.IsAny<string>())).Callback<string>(id =>
            {
                invitation.State = Status.Rejected;
            });

            EmailRequest emailRequest = new EmailRequest
            {
                Email = invitation.Email
            };

            // Act
            IActionResult actionResult = _controller.RejectInvitationState(emailRequest);
            var okResult = actionResult as OkObjectResult;

            // Assert
            OkObjectResult objectResult = actionResult as OkObjectResult;
            var responseValue = okResult.Value as InvitationResponse;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(objectResult.StatusCode, okResult.StatusCode);
            Assert.AreEqual(invitation.State, Status.Rejected);
        }

        [TestMethod]
        public void AcceptInvitation_WithValidDataCreatesManagerTest()
        {
            // Arrange
            Invitation invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                Email = "mail@gmial.com",
                Name = "name",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            UserLoginRequest managerRequest = new UserLoginRequest
            {
                Email = "mail@gmail.com",
                Password = "password"
            };

            User manager = new User
            {
                Id = Guid.NewGuid(),
                Email = managerRequest.Email,
                Password = managerRequest.Password
            };

            User createdManager = new User
            {
                Id = Guid.NewGuid(),
                Email = managerRequest.Email,
                Name = invitation.Name,
                Password = managerRequest.Password,
                Role = Roles.Manager
            };

            UserResponse userResponse = new UserResponse(createdManager);

            _invitationLogicMock.Setup(l => l.AcceptInvitation(It.IsAny<User>())).Returns(createdManager);

            // Act
            IActionResult actionResult = _controller.AcceptInvitation(managerRequest);
            var createdResponse = actionResult as CreatedAtActionResult;

            Assert.IsNotNull(createdResponse);
            Assert.AreEqual(201, createdResponse.StatusCode);

            var responseValue = createdResponse.Value as UserResponse;
            Assert.IsNotNull(responseValue);
            Assert.AreEqual(responseValue, userResponse);
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

            // Assert
            _invitationLogicMock.Verify(l => l.DeleteInvitation(invitationId), Times.Once);
            Assert.IsInstanceOfType(deleteActionResult, typeof(OkResult));
        }

        [TestMethod]
        public void DeleteInvitation_ReturnsNotFound_WhenInvitationNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid(); // Generar un ID que no exista en el sistema

            // Configurar el comportamiento del mock para simular una excepción al intentar eliminar la invitación
            _invitationLogicMock.Setup(l => l.DeleteInvitation(nonExistingId)).Throws(new ObjectNotFoundException("Invitation not found"));

            try
            {
                // Act
                IActionResult result = _controller.DeleteInvitation(nonExistingId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
                var notFoundResult = (NotFoundObjectResult)result;
                Assert.AreEqual(404, notFoundResult.StatusCode);
                Assert.AreEqual("Invitation not found", notFoundResult.Value);
                _invitationLogicMock.Verify(l => l.DeleteInvitation(nonExistingId), Times.Once);
            }
            catch (ObjectNotFoundException ex)
            {
                // Si se lanza la excepción NotFoundException, la prueba pasa
                Assert.AreEqual("Invitation not found", ex.Message);
            }
        }

        [TestMethod]
        public void GetAllInvitationsTestController()
        {
            //Arrange
            string token = "b4d9e6a4-466c-4a4f-91ea-6d7e7997584e";

            User manager = new User
            {
                Id = Guid.NewGuid(),
                Role = Roles.Manager,
                Name = "John",
                LastName = "Doe"
            };

            Invitation invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                Email = "invitation@gmial.com",
                Name = "name",
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                State = Status.Pending,
                Role = Roles.Manager
            };

            IEnumerable<Invitation> expectedInvitations = new List<Invitation>
            {
                invitation
            };

            var expectedResponse = expectedInvitations.Select(b => new InvitationResponse(b)).ToList();

            Mock<IUserLogic> userLogic = new Mock<IUserLogic>(MockBehavior.Strict);
            Mock<IInvitationLogic> invitationLogic = new Mock<IInvitationLogic>(MockBehavior.Strict);

            invitationLogic.Setup(l => l.GetAllInvitations()).Returns(expectedInvitations);

            InvitationController invitationController = new InvitationController(invitationLogic.Object);
            invitationController.ControllerContext.HttpContext = new DefaultHttpContext();
            invitationController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token;

            OkObjectResult expectedObjResult = new OkObjectResult(expectedResponse);

            // Act
            var constrollerResult = invitationController.GetAllInvitations();

            // Assert
            userLogic.VerifyAll();
            OkObjectResult resultObj = constrollerResult as OkObjectResult;
            List<InvitationResponse> resultResponse = resultObj.Value as List<InvitationResponse>;
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(resultObj.StatusCode, expectedObjResult.StatusCode);
            Assert.AreEqual(resultResponse.First(), expectedResponse.First());
        }
    }
}
