using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
using Domain.@enum;
using CustomExceptions;
namespace BusinessLogicTest
{
    [TestClass]
    public class ConstructionCompanyLogicTest
    {
        [TestMethod]
        public void CreateConstructionCompany_ShouldReturnConstructionCompany()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var expectedConstructionCompany = new ConstructionCompany()
            {
                Name = "Test",
                ConstructionCompanyAdmin = new User()
                {
                    Id = adminId,  // Asegurando que el ID del administrador esté establecido
                    Email = "lkand@gmail.com",
                    Password = "1234",
                    Role = Roles.ConstructionCompanyAdmin,
                    Name = "Lukas",
                    LastName = "Kand"
                }
            };

            var repoMock = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            repoMock.Setup(l => l.GetConstructionCompanyByName(It.IsAny<string>())).Returns((string name) => null); 
            repoMock.Setup(l => l.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(expectedConstructionCompany); 
            repoMock.Setup(l => l.GetConstructionCompanyByAdmin(adminId)).Returns(false);  

            var constructionCompanyLogic = new ConstructionCompanyLogic(repoMock.Object);

            var result = constructionCompanyLogic.CreateConstructionCompany(expectedConstructionCompany);

            repoMock.VerifyAll(); 
            Assert.AreEqual(expectedConstructionCompany, result); 
        }
    }
}