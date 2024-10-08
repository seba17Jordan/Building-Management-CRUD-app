﻿using LogicInterface;
using Domain;
using Domain.@enum;
using Moq;
using BusinessLogic;
using IDataAccess;
using CustomExceptions;
namespace BusinessLogicTest
{
    [TestClass]
    public class InvitationLogicTest
    {
        [TestMethod]
        public void CreateInvitationLogicTest()
        {
            // Arrange
            var mockInvitationRepository = new Mock<IInvitationRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);

            var invitation = new Invitation()
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            mockInvitationRepository.Setup(x => x.CreateInvitation(invitation)).Returns(invitation);
            // Act
            var result = invitationLogic.CreateInvitation(invitation);
            // Assert
            Assert.AreEqual(invitation, result);
        }

        [TestMethod]
        public void RejectInvitationStateLogicTest()
        {
            // Arrange
            Invitation invitation = new Invitation()
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6),
                State = Status.Pending
            };

            var mockInvitationRepository = new Mock<IInvitationRepository>();
            var mockUserRepository = new Mock<IUserRepository>();

            InvitationLogic invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);
            mockInvitationRepository.Setup(x => x.GetInvitationByMail(It.IsAny<string>())).Returns(invitation);
            mockInvitationRepository.Setup(x => x.UpdateInvitation(It.IsAny<Invitation>()));

            // Act
            invitationLogic.RejectInvitation(invitation.Email);

            // Assert
            Assert.AreEqual(Status.Rejected, invitation.State);
        }

        [TestMethod]
        public void DeleteInvitationLogicTest()
        {
            // Arrange
            var mockInvitationRepository = new Mock<IInvitationRepository>();
            var mockUserRepository = new Mock<IUserRepository>();

            var invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);

            // Mock the behavior of GetInvitationById to return an invitation
            var invitationId = Guid.NewGuid();
            Invitation invitation = new Invitation()
            {
                Id = invitationId,
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6),
                State = Status.Rejected
            };
            mockInvitationRepository.Setup(x => x.GetInvitationById(invitationId)).Returns(invitation);

            // Act
            invitationLogic.DeleteInvitation(invitationId);

            // Assert
            mockInvitationRepository.Verify(x => x.DeleteInvitation(invitationId), Times.Once);
        }

        [TestMethod]
        public void AcceptInvitationLogicTest()
        {
            // Arrange
            Invitation invitation = new Invitation()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Email = "mail@gmail.com",
                ExpirationDate = DateTime.Now.AddDays(6),
                State = Status.Pending
            };

            User managerToCreate = new User()
            {
                Id = Guid.NewGuid(),
                Email = "mail@gmail.com",
                Password = "password",
                Role = Roles.Manager,
                Name = "John"
            };

            User expectedManager = new User()
            {
                Id = Guid.NewGuid(),
                Email = "mail@gmail.com",
                Password = "password",
                Role = Roles.Manager,
                Name = "John"
            };

            var mockInvitationRepository = new Mock<IInvitationRepository>();
            var mockUserRepository = new Mock<IUserRepository>();

            mockInvitationRepository.Setup(x => x.GetInvitationByMail(invitation.Email)).Returns(invitation);
            mockInvitationRepository.Setup(x => x.UpdateInvitation(invitation));
            mockUserRepository.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(expectedManager);

            InvitationLogic invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);


            // Act
            User result = invitationLogic.AcceptInvitation(managerToCreate);

            // Assert
            mockInvitationRepository.Verify();
            mockUserRepository.Verify();
            Assert.AreEqual(Status.Accepted, invitation.State);
            Assert.AreEqual(expectedManager, result);
        }

        [TestMethod]
        public void CreateInvitation_InvalidEmail()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IUserRepository> repo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                Invitation invitation = new Invitation()
                {
                    Id = Guid.NewGuid(),
                    Name = "John",
                    Email = "magailcom",
                    ExpirationDate = DateTime.Now.AddDays(6),
                    State = Status.Pending
                };

                var mockInvitationRepository = new Mock<IInvitationRepository>();
                var mockUserRepository = new Mock<IUserRepository>();

                InvitationLogic invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);

                // Act
                invitationLogic.CreateInvitation(invitation);
            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Invalid email format"));
        }

        [TestMethod]
        public void CreateInvitation_EmailAlreadyExists()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IUserRepository> repo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                Invitation invitation = new Invitation()
                {
                    Id = Guid.NewGuid(),
                    Name = "John",
                    Email = "magail@aasa.com",
                    ExpirationDate = DateTime.Now.AddDays(6),
                    State = Status.Pending
                };

                var mockInvitationRepository = new Mock<IInvitationRepository>();
                var mockUserRepository = new Mock<IUserRepository>();
                mockInvitationRepository.Setup(x => x.InvitationExists(It.IsAny<string>())).Returns(true);

                InvitationLogic invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);

                // Act
                invitationLogic.CreateInvitation(invitation);
            }
            catch (ObjectAlreadyExistsException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ObjectAlreadyExistsException));
            Assert.IsTrue(specificEx.Message.Contains("Invitation with same Email already exists"));
        }

        [TestMethod]
        public void AcceptInvitationExpiredTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IUserRepository> repo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                User managerToCreate = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "magail@aasa.com",
                    Password = "password",
                    Role = Roles.Manager,
                    Name = "John"
                };

                Invitation invitation = new Invitation()
                {
                    Id = Guid.NewGuid(),
                    Name = "John",
                    Email = "magail@aasa.com",
                    ExpirationDate = new DateTime(2022,05,05),
                    State = Status.Pending
                };

                var mockInvitationRepository = new Mock<IInvitationRepository>();
                var mockUserRepository = new Mock<IUserRepository>();

                mockInvitationRepository.Setup(x => x.GetInvitationByMail(invitation.Email)).Returns(invitation);

                InvitationLogic invitationLogic = new InvitationLogic(mockInvitationRepository.Object, mockUserRepository.Object);

                // Act
                invitationLogic.AcceptInvitation(managerToCreate);
            }
            catch (InvalidOperationException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(InvalidOperationException));
            Assert.IsTrue(specificEx.Message.Contains("The invitation has expired"));
        }

        [TestMethod]
        public void GetAllInvitationsCorrectTestLogic()
        {
            // Arrange
            var invitations = new List<Invitation>
            {
                new Invitation {
                    Id = Guid.NewGuid(),
                    Name = "Manager 1",
                    Role = Roles.Manager,
                    Email = "",
                    ExpirationDate = DateTime.Now.AddDays(6),
                    State = Status.Pending
                },
                new Invitation {
                    Id = Guid.NewGuid(),
                    Name = "Manager 1",
                    Role = Roles.Manager,
                    Email = "dae",
                    ExpirationDate = DateTime.Now.AddDays(6),
                    State = Status.Accepted
                }
            };

            Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
            Mock<IInvitationRepository> _invitationRepository = new Mock<IInvitationRepository>();

            _invitationRepository.Setup(x => x.GetAllInvitations()).Returns(invitations);

            // Act
            InvitationLogic _invitationLogic = new InvitationLogic(_invitationRepository.Object, _userRepository.Object);

            var result = _invitationLogic.GetAllInvitations();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Manager 1", result.First().Name);
        }
    }
}