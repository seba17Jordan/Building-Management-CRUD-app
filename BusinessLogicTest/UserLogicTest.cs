using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
namespace BusinessLogicTest
{
    [TestClass]
    public class UserLogicTest
    {
        [TestMethod]
        public void CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Federico",
                Email = "test@example.com",
                Password = "password",
                Role = "Admin"
            };

            Mock<IUserRepository> logic = new Mock<IUserRepository>(MockBehavior.Strict);
            logic.Setup(l => l.CreateUser(user)).Returns(user);
            var userLogic = new UserLogic(logic.Object);

            // Act
            var result = userLogic.CreateUser(user);

            // Assert
            logic.VerifyAll();
            Assert.AreEqual(user.Id, result.Id);

        }
    }
}