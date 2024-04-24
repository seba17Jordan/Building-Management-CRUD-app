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
    public class CategoryControllerTests
    {
        private Mock<ICategoryLogic> _categoryLogicMock;
        private CategoryController _controller;

        [TestInitialize]
        public void Setup()
        {
            _categoryLogicMock = new Mock<ICategoryLogic>(MockBehavior.Strict);
            _controller = new CategoryController(_categoryLogicMock.Object);
        }

        [TestMethod]
        public void CreateCategory_WithValidData_Test()
        {
            // Arrange
            var categoryRequest = new CategoryRequest
            {
                Name = "TestCategory"
            };

            var createdCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryRequest.Name
            };

            _categoryLogicMock.Setup(l => l.CreateCategory(It.IsAny<Category>())).Returns(createdCategory);

            var expectedMappedCategory = new CategoryResponse(createdCategory);

            // Act
            var actionResult = _controller.CreateCategory(categoryRequest) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(actionResult);

            var categoryResponse = actionResult.Value as CategoryResponse;
            Assert.AreEqual(expectedMappedCategory, categoryResponse);        }
    }
}
