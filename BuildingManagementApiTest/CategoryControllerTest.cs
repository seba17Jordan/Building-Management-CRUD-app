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

        [TestMethod]
        public void GetAllCategories_ReturnsCategories_Successfully()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category1" },
                new Category { Id = Guid.NewGuid(), Name = "Category2" },
                new Category { Id = Guid.NewGuid(), Name = "Category3" }
            };

            var expectedCategoryResponses = categories.Select(c => new CategoryResponse(c)).ToList();

            _categoryLogicMock.Setup(l => l.GetAllCategories()).Returns(categories);

            // Act
            var actionResult = _controller.GetAllCategories() as OkObjectResult;

            // Assert
            Assert.IsNotNull(actionResult);

            var categoryResponses = actionResult.Value as List<CategoryResponse>;
            Assert.IsNotNull(categoryResponses);

            Assert.AreEqual(expectedCategoryResponses.Count, categoryResponses.Count);

            for (int i = 0; i < expectedCategoryResponses.Count; i++)
            {
                Assert.AreEqual(expectedCategoryResponses[i].Id, categoryResponses[i].Id);
                Assert.AreEqual(expectedCategoryResponses[i].Name, categoryResponses[i].Name);
            }
        }
    }
}
