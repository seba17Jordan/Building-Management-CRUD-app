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

        [TestMethod]
        public void GetUserById_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Federico",
                Email = "example@hotmail.com",
                Password = "password",
                Role = "Admin"
            };

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object);

            // Act
            var result = userLogic.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
        }

    }
}