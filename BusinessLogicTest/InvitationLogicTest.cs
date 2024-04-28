using LogicInterface;
using Domain;
using Domain.@enum;
using Moq;
using BusinessLogic;
using IDataAccess;
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
            var invitationLogic = new InvitationLogic(mockInvitationRepository.Object);

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
            
            InvitationLogic invitationLogic = new InvitationLogic(mockInvitationRepository.Object);
            mockInvitationRepository.Setup(x => x.GetInvitationById(It.IsAny<Guid>())).Returns(invitation);
            mockInvitationRepository.Setup(x => x.UpdateInvitation(It.IsAny<Invitation>()));
            
            // Act
            invitationLogic.RejectInvitation(invitation.Id);
            
            // Assert
            Assert.AreEqual(Status.Rejected, invitation.State);
        }

        [TestMethod]
        public void DeleteInvitationLogicTest()
        {
            // Arrange
            var mockInvitationRepository = new Mock<IInvitationRepository>();
            var invitationLogic = new InvitationLogic(mockInvitationRepository.Object);

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
    }
}