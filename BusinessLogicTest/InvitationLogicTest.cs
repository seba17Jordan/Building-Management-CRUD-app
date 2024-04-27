using LogicInterface;
using Domain;
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
    }
}