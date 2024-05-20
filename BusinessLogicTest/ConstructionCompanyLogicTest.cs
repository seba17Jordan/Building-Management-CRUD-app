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
            var expectedConstructionCompany = new ConstructionCompany
            {
                Id = Guid.NewGuid(),
                Name = "ConstructionCompany",
            };

            var repoMock = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            repoMock.Setup(l => l.GetConstructionCompanyByName(It.IsAny<string>())).Returns((ConstructionCompany)null);
            repoMock.Setup(l => l.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(expectedConstructionCompany);

            var constructionCompanyLogic = new ConstructionCompanyLogic(repoMock.Object);

            // Act
            var result = constructionCompanyLogic.CreateConstructionCompany(expectedConstructionCompany);

            // Assert
            repoMock.VerifyAll();
            Assert.AreEqual(expectedConstructionCompany, result);
        }
    }
}