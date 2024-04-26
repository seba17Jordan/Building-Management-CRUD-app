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
            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Federico",
                Email = "test@example.com",
                Password = "password",
                Role = Domain.@enum.Roles.Administrator
            };

            Mock<IUserRepository> repo = new Mock<IUserRepository>(MockBehavior.Strict);
            repo.Setup(l => l.CreateUser(It.IsAny<User>())).Returns(expectedUser);

            var userLogic = new UserLogic(repo.Object);

            // Act
            var result = userLogic.CreateUser(expectedUser);

            // Assert
            repo.VerifyAll();
            Assert.AreEqual(expectedUser, result);
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
                Role = Domain.@enum.Roles.Administrator
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

        [TestMethod]
        public void UpdateUser_ShouldReturnUpdatedUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updatedName = "UpdatedName";
            var updatedEmail = "updated@example.com";
            var updatedPassword = "updatedPassword";
            var updatedRole = Domain.@enum.Roles.Manager;

            var user = new User
            {
                Id = userId,
                Name = "Federico",
                Email = "example@hotmail.com",
                Password = "password",
                Role = Domain.@enum.Roles.Administrator
            };

            var updatedUser = new User
            {
                Id = userId,
                Name = updatedName,
                Email = updatedEmail,
                Password = updatedPassword,
                Role = updatedRole
            };

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);
            userRepositoryMock.Setup(repo => repo.UpdateUser(updatedUser)).Returns(updatedUser);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object);

            // Act
            var result = userLogic.UpdateUser(updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(updatedName, result.Name);
            Assert.AreEqual(updatedEmail, result.Email);
            Assert.AreEqual(updatedPassword, result.Password);
            Assert.AreEqual(updatedRole, result.Role);
        }
    }
}