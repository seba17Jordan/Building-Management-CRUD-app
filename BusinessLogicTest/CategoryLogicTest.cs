﻿using LogicInterface;
using Domain;
using Moq;
using BusinessLogic;
using IDataAccess;
namespace BusinessLogicTest
{
    [TestClass]
    public class CategoryLogicTest
    {
        [TestMethod]
        public void CreateCategory_ShouldReturnCreatedCategoryTest()
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
    }
}