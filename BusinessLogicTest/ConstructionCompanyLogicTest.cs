﻿using LogicInterface;
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
            User construectionCompanyUser = new User()
            {
                Id = Guid.NewGuid(),  // Asegurando que el ID del administrador esté establecido
                Email = "lkand@gmail.com",
                Password = "1234",
                Role = Roles.ConstructionCompanyAdmin,
                Name = "Lukas",
                LastName = "Kand"
            };

            ConstructionCompany expectedConstructionCompany = new ConstructionCompany()
            {
                Name = "Test",
                ConstructionCompanyAdmin = construectionCompanyUser
            };

            Mock< IConstructionCompanyRepository> repoMock = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            repoMock.Setup(l => l.GetConstructionCompanyByName(It.IsAny<string>())).Returns((ConstructionCompany)null); 
            repoMock.Setup(l => l.CreateConstructionCompany(It.IsAny<ConstructionCompany>())).Returns(expectedConstructionCompany); 
            repoMock.Setup(l => l.GetConstructionCompanyByAdmin(It.IsAny<Guid>())).Returns((ConstructionCompany)null);  

            var constructionCompanyLogic = new ConstructionCompanyLogic(repoMock.Object);

            var result = constructionCompanyLogic.CreateConstructionCompany(expectedConstructionCompany);

            repoMock.VerifyAll(); 
            Assert.AreEqual(expectedConstructionCompany, result); 
        }
        
        [TestMethod]
        public void UpdateConstructionCompanyNameCorrectLogicTest()
        {
            //Arrange
            User constructionCompanyAdmin = new User()
            {
                Id = Guid.NewGuid(),
                Email = "saedf@gmail.com",
                Password = "1234",
                Role = Roles.ConstructionCompanyAdmin,
                Name = "Lukas",
                LastName = "Kand"
            };

            ConstructionCompany constructionCompany = new ConstructionCompany()
            {
                Name = "Test",
                ConstructionCompanyAdmin = constructionCompanyAdmin
            };
            
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>();
            constructionCompanyRepo.Setup(x => x.GetConstructionCompanyByAdmin(constructionCompanyAdmin.Id)).Returns(constructionCompany);
            constructionCompanyRepo.Setup(x => x.GetConstructionCompanyByName(It.IsAny<string>())).Returns((ConstructionCompany)null);
            constructionCompanyRepo.Setup(x => x.UpdateConstructionCompany(It.IsAny<ConstructionCompany>()))
                .Returns(constructionCompany)
                .Callback<ConstructionCompany>(x => x.Name = "NewName");

            ConstructionCompanyLogic constructionCompanyLogic = new ConstructionCompanyLogic(constructionCompanyRepo.Object);

            //Act
            ConstructionCompany logicResult = constructionCompanyLogic.UpdateConstructionCompanyName("NewName", constructionCompanyAdmin);

            //Assert
            Assert.AreEqual(logicResult.Name, "NewName");
        }

        [TestMethod]
        public void UpdateConstructionCompanyNameAlreadyExists()
        {
            //Arrange
            Exception specificEx = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            try
            {
                //Arrange
                User constructionComAdmin = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    LastName = "ConstructionCompAdmin",
                    Email = "aknd@gmail.com",
                    Role = Roles.ConstructionCompanyAdmin,
                };

                ConstructionCompany constructionCompany = new ConstructionCompany()
                {
                    Name = "Construction Company",
                    ConstructionCompanyAdmin = constructionComAdmin
                };

                ConstructionCompany existingConstructionCompany = new ConstructionCompany()
                {
                    Name = "Existing Construction Company",
                    ConstructionCompanyAdmin = constructionComAdmin
                };

                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>();

                constructionCompanyRepo.Setup(x => x.GetConstructionCompanyByAdmin(It.IsAny<Guid>())).Returns(constructionCompany);
                constructionCompanyRepo.Setup(x => x.GetConstructionCompanyByName(It.IsAny<string>())).Returns(constructionCompany);

                ConstructionCompanyLogic constructionCompanyLogic = new ConstructionCompanyLogic(constructionCompanyRepo.Object);

                // Act
                ConstructionCompany logicResult = constructionCompanyLogic.UpdateConstructionCompanyName("NewName", constructionComAdmin);

            }
            catch (ObjectAlreadyExistsException e)
            {
                specificEx = e;
            }

            // Assert
            constructionCompanyRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ObjectAlreadyExistsException));
            Assert.IsTrue(specificEx.Message.Contains("Construction company with that name already exists."));
        }

        [TestMethod]
        public void GetConstructionCompanyCorrectLogicTest()
        {
            User constructionComAdmin = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                LastName = "ConstructionCompAdmin",
                Email = "aknd@gmail.com",
                Role = Roles.ConstructionCompanyAdmin,
            };

            ConstructionCompany constructionCompany = new ConstructionCompany()
            {
                Name = "Construction Company",
                ConstructionCompanyAdmin = constructionComAdmin
            };

            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>();
            constructionCompanyRepo.Setup(x => x.GetConstructionCompanyByAdmin(It.IsAny<Guid>())).Returns(constructionCompany);

            ConstructionCompanyLogic constructionCompanyLogic = new ConstructionCompanyLogic(constructionCompanyRepo.Object);

            //Act
            ConstructionCompany logicResult = constructionCompanyLogic.GetConstructionCompany(constructionComAdmin);
            Assert.IsNotNull(logicResult);
            Assert.AreEqual(logicResult, constructionCompany);
        }
    }
}