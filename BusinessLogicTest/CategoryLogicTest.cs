using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
using Domain.@enum;
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
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            repo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Category name can't be null or empty"));
        }
    }
}