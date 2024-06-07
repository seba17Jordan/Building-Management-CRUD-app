using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
using CustomExceptions;
using Domain.@enum;
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
            catch(ObjectAlreadyExistsException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ObjectAlreadyExistsException));
            Assert.AreEqual("Email already exists", specificEx.Message);
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
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Invalid email format"));
        }

        [TestMethod]
        public void CreateUser_InvalidPassword()
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
                    Password = "pass",
                    Role = Domain.@enum.Roles.Administrator
                };

                var userLogic = new UserLogic(repo.Object);

                // Act
                var logicResult = userLogic.CreateUser(expectedUser);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Password must be at least 6 characters long"));
        }

        [TestMethod]
        public void GetManagersCorrectTestLogic()
        {
            // Arrange
            var managers = new List<User>
            {
                new User {
                    Id = Guid.NewGuid(),
                    Name = "Manager 1",
                    Role = Roles.Manager,
                    Email = ""
                },
                new User {
                    Id = Guid.NewGuid(),
                    Name = "Manager 1",
                    Role = Roles.Manager,
                    Email = "" 
                }
            };

            Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
            _userRepository.Setup(x => x.GetManagers()).Returns(managers);

            // Act
            UserLogic _userLogic = new UserLogic(_userRepository.Object);

            var result = _userLogic.GetAllManagers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Manager 1", result.First().Name);
        }
    }
}