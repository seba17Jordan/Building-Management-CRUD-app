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
        public void UpdateInvitationStateLogicTest()
        {
            // Arrange
            var mockInvitationRepository = new Mock<IInvitationRepository>();
            var invitationLogic = new InvitationLogic(mockInvitationRepository.Object);

            Invitation invitation = new Invitation()
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            mockInvitationRepository.Setup(x => x.GetInvitationById(It.IsAny<Guid>())).Returns(invitation);
            mockInvitationRepository.Setup(x => x.UpdateInvitation(invitation)).Returns(invitation);
            // Act
            var result = invitationLogic.UpdateInvitationState(Guid.NewGuid(), Status.Accepted);
            // Assert
            Assert.AreEqual(Status.Accepted, result.State);
        }
    }
}