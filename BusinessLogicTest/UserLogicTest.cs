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
            repo.Setup(repo => repo.UserExists(It.IsAny<Func<User, bool>>())).Returns(false);

            var userLogic = new UserLogic(repo.Object);

            // Act
            var result = userLogic.CreateUser(expectedUser);

            // Assert
            repo.VerifyAll();
            Assert.AreEqual(expectedUser, result);
        }

        [TestMethod]
        public void CreateUser_AlreadyExists()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IUserRepository> repo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                User expectedUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Federico",
                    Email = "test@example.com",
                    Password = "password",
                    Role = Domain.@enum.Roles.Administrator
                };
                repo.Setup(repo => repo.UserExists(It.IsAny<Func<User,bool>>())).Returns(true);
                var userLogic = new UserLogic(repo.Object);

                // Act
                var logicResult = userLogic.CreateUser(expectedUser);

            }
            catch(ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));   //Crear exception especifica
            Assert.AreEqual("User already exists", specificEx.Message);
        }

        [TestMethod]
        public void CreateUser_InvalidEmail()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IUserRepository> repo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                User expectedUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Federico",
                    Email = "testexample.com",
                    Password = "password",
                    Role = Domain.@enum.Roles.Administrator
                };
                repo.Setup(repo => repo.UserExists(It.IsAny<Func<User,bool>>())).Returns(false);
                var userLogic = new UserLogic(repo.Object);

                // Act
                var logicResult = userLogic.CreateUser(expectedUser);

            }
            catch(ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));   //Crear exception especifica
            Assert.IsTrue(specificEx.Message.Contains("Invalid email format"));
        }       
    }
}