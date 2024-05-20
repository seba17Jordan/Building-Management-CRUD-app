using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LogicInterface;
using ModelsApi.In;
using ModelsApi.Out;
using BuildingManagementApi.Controllers;
using Domain;

namespace BuildingManagementApi.Tests.Controllers
{
    [TestClass]
    public class ConstructionCompanyControllerTest
    {
        private Mock<IConstructionCompanyLogic> _constructionCompanyLogic;
        private ConstructionCompanyController _controller;

        [TestInitialize]
        public void Setup()
        {
            _constructionCompanyLogic = new Mock<IConstructionCompanyLogic>(MockBehavior.Strict);
            _controller = new ConstructionCompanyController(_constructionCompanyLogic.Object);
        }

        [TestMethod]
        public void CreateConstructionCompanyTest()
        {
            // Arrange
            var constructionCompanyRequest = new ConstructionCompanyRequest
            {
                Name = "Company"
            };

            var constructionCompany = new ConstructionCompany
            {
                Id = new System.Guid(),
                Name = "Company"
            };

            _constructionCompanyLogic.Setup(x => x.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(constructionCompany);

            // Act
            var result = _controller.CreateConstructionCompany(constructionCompanyRequest) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual("CreateConstructionCompany", result.ActionName);
            Assert.AreEqual(constructionCompany.Id, (result.Value as ConstructionCompanyResponse).Id);
            Assert.AreEqual(constructionCompany.Name, (result.Value as ConstructionCompanyResponse).Name);
        }
    }
}
