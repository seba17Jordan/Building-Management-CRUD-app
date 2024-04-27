using DataAccess;
using Domain;
using Domain.@enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void CreateUserAdministratorCorrectTest()
        {
            // Arrange
            var expectedUser = new User
            {
                Name = "name",
                LastName = "last name",
                Email = "admin@gmail.com",
                Password = "123456",
                Role = Roles.Administrator
            };
            
            var context = CreateDbContext("CreateUserAdministratorCorrectTest");
            var userRepository = new UserRepository(context);

            // Act
            User createdUser = userRepository.CreateUser(expectedUser);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedUser, createdUser);
        }

        [TestMethod]
        public void GetUserByEmailCorrectTest()
        {
            // Arrange
            User expectedUser = new User
            {
                Name = "name",
                LastName = "last name",
                Email = "mail@gmail.com",
                Password = "123456",
                Role = Roles.Administrator
            };

            var context = CreateDbContext("GetUserByEmailCorrectTest");
            var userRepository = new UserRepository(context);
            userRepository.CreateUser(expectedUser);
            context.SaveChanges();

            // Act
            User userFound = userRepository.GetUserByEmail("mail@gmail.com");
            

            // Assert
            Assert.AreEqual(userFound, expectedUser);
        }

        [TestMethod]
        public void GetUserByIdDCorrectTest()
        {
            // Arrange
            User expectedUser = new User
            {
                Name = "name",
                LastName = "last name",
                Email = "mail@gmail.com",
                Password = "123456",
                Role = Roles.Administrator
            };

            Guid expectedId = expectedUser.Id;
            var context = CreateDbContext("GetUserByIdDCorrectTest");
            var userRepository = new UserRepository(context);
            userRepository.CreateUser(expectedUser);
            context.SaveChanges();

            // Act
            User userFound = userRepository.GetUserById(expectedId);


            // Assert
            Assert.AreEqual(userFound, expectedUser);
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}