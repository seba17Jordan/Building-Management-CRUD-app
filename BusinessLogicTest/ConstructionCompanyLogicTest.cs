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

            Mock<IConstructionCompanyRepository> repo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            repo.Setup(l => l.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(expectedConstructionCompany);

            var constructionCompanyLogic = new ConstructionCompanyLogic(repo.Object);

            // Act
            var result = constructionCompanyLogic.CreateConstructionCompany(expectedConstructionCompany);

            // Assert
            repo.VerifyAll();
            Assert.AreEqual(expectedConstructionCompany, result);
        }
    }
}