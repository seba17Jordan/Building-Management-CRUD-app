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
    public class CategoryLogicTest
    {
        [TestMethod]
        public void CreateCategory_ShouldReturnCreatedCategoryTestLogic()
        {
            // Arrange
            var expectedCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category"
            };

            Mock<ICategoryRepository> repo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            repo.Setup(l => l.CreateCategory(It.IsAny<Category>())).Returns(expectedCategory);
            repo.Setup(repo => repo.FindCategoryByName(It.IsAny<string>())).Returns(false);

            var categoryLogic = new CategoryLogic(repo.Object);

            // Act
            var result = categoryLogic.CreateCategory(expectedCategory);

            // Assert
            repo.VerifyAll();
            Assert.AreEqual(expectedCategory, result);
        }

        [TestMethod]
        public void CreateCategory_EmptyNameTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<ICategoryRepository> repo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            try
            {
                Category cat = new Category()
                {
                    Name = ""
                };

                var caegoryMockRepository = new Mock<ICategoryRepository>(MockBehavior.Strict);
                CategoryLogic categoryLogic = new CategoryLogic(repo.Object);

                // Act
                categoryLogic.CreateCategory(cat);
            }
            catch (EmptyFieldException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(EmptyFieldException));
            Assert.IsTrue(specificEx.Message.Contains("Category name can't be empty"));
        }

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

            Mock<ICategoryRepository> repositoryMock = new Mock<ICategoryRepository>(MockBehavior.Strict);
            repositoryMock.Setup(repo => repo.GetAllCategories()).Returns(categories);

            var categoryLogic = new CategoryLogic(repositoryMock.Object);

            // Act
            var result = categoryLogic.GetAllCategories();

            // Assert
            repositoryMock.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(categories.Count, result.Count());

            foreach (var category in categories)
            {
                Assert.IsTrue(result.Any(c => c.Id == category.Id && c.Name == category.Name));
            }
        }
    }
}