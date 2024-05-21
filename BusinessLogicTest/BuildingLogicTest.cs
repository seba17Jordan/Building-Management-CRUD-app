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
    public class BuildingLogicTest
    {
        [TestMethod]
        public void CreateBuildingCorrectTestLogic()
        {
            //Arrange
            User constructionCompanyAdmin = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                LastName = "ConstructionCompAdmin",
                Email = "adeae@gmai.com",
                Role = Roles.ConstructionCompanyAdmin,
            };

            ConstructionCompany constructionCompany = new ConstructionCompany()
            {
                Name = "Construction Company",
                ConstructionCompanyAdmin = constructionCompanyAdmin
            };

            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = constructionCompany,
                CommonExpenses = 100,
                ConstructionCompanyAdmin = constructionCompanyAdmin,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                }
            };

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            buildingRepo.Setup(l => l.CreateBuilding(It.IsAny<Building>())).Returns(expectedBuilding);
            buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);
            constructionCompanyRepo.Setup(l => l.GetConstructionCompanyByName(It.IsAny<string>())).Returns(constructionCompany);

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            //Act
            Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            //Assert
            buildingRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedBuilding);
        }

        [TestMethod]
        public void CreateBuildingNullShouldThrowNullExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(null);

            }
            catch (ArgumentNullException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentNullException));   //Crear exception especifica
            Assert.IsTrue(specificEx.Message.Contains("Building is null"));
        }

        [TestMethod]
        public void CreateBuildingInvalidNameThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                }
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (EmptyFieldException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(EmptyFieldException));
            Assert.IsTrue(specificEx.Message.Contains("There are empty fields"));
        }

        [TestMethod]
        public void CreateBuildingInvalidAddressThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                }
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (EmptyFieldException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(EmptyFieldException));
            Assert.IsTrue(specificEx.Message.Contains("There are empty fields"));
        }

        [TestMethod]
        public void CreateBuildingInvalidCompanyThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany(""),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                }
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (EmptyFieldException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(EmptyFieldException));
            Assert.IsTrue(specificEx.Message.Contains("There are empty fields"));
        }

        [TestMethod]
        public void CreateBuildingNoApartmentsThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange
                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>()
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building must have at least one apartment"));
        }

        [TestMethod]
        public void CreateBuildingInvalidExpensesThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange
                Apartment apt = new Apartment()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "dadae@gmai.com" },
                    Rooms = 3,
                    Bathrooms = 2,
                    HasTerrace = true
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = -100,
                    Apartments = new List<Apartment>()
                    {
                        apt
                    }
                };
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Common expenses must be greater than 0"));
        }

        [TestMethod]
        public void CreateBuildingRepeatedNameThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            try
            {
                //Arrange

                Apartment apt = new Apartment()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "dadae@gmai.com" },
                    Rooms = 3,
                    Bathrooms = 2,
                    HasTerrace = true
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>()
                    {
                        apt
                    }
                };
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(true);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ObjectAlreadyExistsException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ObjectAlreadyExistsException));
            Assert.IsTrue(specificEx.Message.Contains("Building with same name already exists"));
        }

        [TestMethod]
        public void DeleteBuildingByIdCorrectTestLogic()
        {
            // Arrange
            User manager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                LastName = "Manager",
                Email = "mai@gmail.com"
            };

            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Name 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                },
                managerId = manager.Id
            };
            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.GetNoClosedServiceRequestsByBuildingId(It.IsAny<Guid>())).Returns(serviceRequests);
            buildingRepo.Setup(l => l.DeleteBuilding(It.IsAny<Building>()));
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(expectedBuilding);
            buildingRepo.Setup(l => l.Save());
            buildingRepo.Setup(l => l.DeleteApartment(It.IsAny<Apartment>()));
            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            // Act
            buildingLogic.DeleteBuildingById(expectedBuilding.Id, manager.Id);

            // Assert
            buildingRepo.VerifyAll();
        }

        [TestMethod]
        public void DeleteBuildingByIdWithApartmentAndOwnerCorrectTestLogic()
        {
            // Arrange
            User manager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                LastName = "Manager",
                Email = "mai@gmail.com"
            };

            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Name 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                },
                managerId = manager.Id
            };
            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            serviceRequestRepo.Setup(l => l.GetNoClosedServiceRequestsByBuildingId(It.IsAny<Guid>())).Returns(serviceRequests);
            buildingRepo.Setup(l => l.DeleteBuilding(It.IsAny<Building>()));
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(expectedBuilding);
            buildingRepo.Setup(l => l.Save());
            buildingRepo.Setup(l => l.DeleteApartment(It.IsAny<Apartment>()));
            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            // Act
            buildingLogic.DeleteBuildingById(expectedBuilding.Id, manager.Id);

            // Assert
            buildingRepo.VerifyAll();
        }

        [TestMethod]
        public void DeleteBuildingByIdEmptyIdShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            try
            {
                // Act
                buildingLogic.DeleteBuildingById(Guid.Empty, Guid.NewGuid());
            }
            catch (EmptyFieldException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(EmptyFieldException));
            Assert.IsTrue(specificEx.Message.Contains("Id is empty"));
        }

        [TestMethod]
        public void DeleteBuildingWithAssociatedActiveServiceRequestExceptionTestLogic()
        {
            // Arrange
            User manager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                LastName = "Manager",
                Email = "srff@gmail.com",
                Role = Roles.Manager,
                Password = "123456"
            };

            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Name 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "daedeq@gmaui.com"}
                    }
                },
                managerId = manager.Id
            };

            Category category = new Category()
            {
                Name = "Category"
            };

            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Id = Guid.NewGuid(),
                BuildingId = building.Id,
                Description = "Description",
                Status = ServiceRequestStatus.Open,
                Apartment = building.Apartments.First().Id,
                Category = category.Id
            };

            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            
            serviceRequestRepo.Setup(l => l.GetNoClosedServiceRequestsByBuildingId(It.IsAny<Guid>())).Returns(new List<ServiceRequest> { serviceRequest });
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(building);

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            try
            {
                // Act
                buildingLogic.DeleteBuildingById(building.Id, manager.Id);
            }
            catch (InvalidOperationException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(InvalidOperationException));
            Assert.IsTrue(specificEx.Message.Contains("There are active service requests associated with this building"));
        }

        [TestMethod]
        public void DeleteBuildingByIdNotFoundShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns((Building)null);
            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            try
            {
                // Act
                buildingLogic.DeleteBuildingById(Guid.NewGuid(), Guid.NewGuid());
            }
            catch (ObjectNotFoundException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ObjectNotFoundException));
            Assert.IsTrue(specificEx.Message.Contains("Building not found"));
        }

        [TestMethod]
        public void UpdateBuildingNameTestLogic()
        {
            //Arrange
            User manager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                LastName = "Manager",
                Email = "sklmf@gmai.com"
            };

            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                },
                managerId = manager.Id

            };

            Building updates = new Building()
            {
                Name = "Building 2",
            };

            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 2",
                Address = "Address 1",
                ConstructionCompany = new ConstructionCompany("Construction Company"),
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
                        Rooms = 3,
                        Bathrooms = 2,
                        HasTerrace = true
                    }
                }
            };

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(building);
            buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);
            buildingRepo.Setup(l => l.UpdateBuilding(It.IsAny<Building>()));
            buildingRepo.Setup(l => l.Save());

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            //Act
            Building logicResult = buildingLogic.UpdateBuildingById(building.Id, updates, manager.Id);

            //Assert
            buildingRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedBuilding);
        }

        [TestMethod]
        public void UpdateBuildingNameRepeatedShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange
                User manager = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Manager",
                    LastName = "Manager",
                    Email = "sef@gmail.com"
                };

                Building building = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = -100,
                    Apartments = new List<Apartment>(),
                    managerId = manager.Id
                };

                Building updates = new Building()
                {
                    Name = "Name 2",
                };

                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(true);
                buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(building);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.UpdateBuildingById(building.Id, updates, manager.Id);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Building with same name already exists"));
        }

        [TestMethod]
        public void UpdateBuildingNotBeingOwnerShouldThrowExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange
                User manager = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Manager",
                    LastName = "Manager",
                    Email = "sef@gmail.com"
                };

                Building building = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = -100,
                    Apartments = new List<Apartment>(),
                };

                Building updates = new Building()
                {
                    Name = "Name 2",
                };

                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(building);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.UpdateBuildingById(building.Id, updates, manager.Id);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("Manager is not the owner of the building"));
        }


        [TestMethod]
        public void CreateBuildingWrongApartmentInfoThrowArgumentExceptionTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange

                Apartment apt = new Apartment()
                {
                    Floor = -1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "dadae@gmai.com" },
                    Rooms = 1,
                    Bathrooms = 2,
                    HasTerrace = true
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Construction Company"),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>()
                    {
                        apt
                    }
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ArgumentException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ArgumentException));
            Assert.IsTrue(specificEx.Message.Contains("All apartment fields must be grater than zero"));
        }

        [TestMethod]
        public void CreateBuildingConstructionCompanyNotFoundThrowExceptionLogicTest()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
            try
            {
                //Arrange

                Apartment apt = new Apartment()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "dadae@gmai.com" },
                    Rooms = 1,
                    Bathrooms = 2,
                    HasTerrace = true
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = new ConstructionCompany("Company"),
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>()
                    {
                        apt
                    }
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                constructionCompanyRepo.Setup(l => l.GetConstructionCompanyByName(It.IsAny<string>())).Returns((ConstructionCompany)null);
                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (ObjectNotFoundException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(ObjectNotFoundException));
            Assert.IsTrue(specificEx.Message.Contains("Construction company not found"));
        }

        [TestMethod]
        public void CreateBuildingConstructionCompanyAdminNotHasConstructionCompanyFound()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
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

                User contructionCompanyAdminNotAllowed = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    LastName = "ConstructionCompAdmin",
                    Email = "cdcs@gmial.com",
                    Role = Roles.ConstructionCompanyAdmin,
                };

                ConstructionCompany constructionCompany = new ConstructionCompany()
                {
                    Name = "Construction Company",
                    ConstructionCompanyAdmin = constructionComAdmin
                };

                Apartment apt = new Apartment()
                {
                    Floor = 1,
                    Number = 101,
                    Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "dadae@gmai.com" },
                    Rooms = 1,
                    Bathrooms = 2,
                    HasTerrace = true
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                    Address = "Address 1",
                    ConstructionCompany = constructionCompany, //(*)
                    CommonExpenses = 100,
                    ConstructionCompanyAdmin = contructionCompanyAdminNotAllowed, //Este no posee la construction company con (*)
                    Apartments = new List<Apartment>()
                    {
                        apt
                    }
                };
                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);


                constructionCompanyRepo.Setup(l => l.GetConstructionCompanyByName(It.IsAny<string>())).Returns(constructionCompany);
                buildingRepo.Setup(l => l.BuildingNameExists(It.IsAny<string>())).Returns(false);

                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.CreateBuilding(expectedBuilding);

            }
            catch (InvalidOperationException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(InvalidOperationException));
            Assert.IsTrue(specificEx.Message.Contains("This construction company does not belong to this construction company administrator"));
        }
        
        [TestMethod]
        public void GetBuildingById_ValidId_ReturnsBuilding()
        {
            // Arrange
            Guid validId = Guid.NewGuid();
            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            var expectedBuilding = new Building
            {
                Id = validId,
                Name = "Test Building",
                Address = "123 Test St",
                ConstructionCompanyAdmin = new User { Id = Guid.NewGuid(), Name = "John", LastName = "Doe" }
            };

            buildingRepo.Setup(x => x.GetBuildingById(validId)).Returns(expectedBuilding);

            // Act
            var result = buildingLogic.GetBuildingById(validId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validId, result.Id);
            Assert.AreEqual(expectedBuilding.Name, result.Name);
            Assert.AreEqual(expectedBuilding.Address, result.Address);
            Assert.AreEqual(expectedBuilding.ConstructionCompanyAdmin, result.ConstructionCompanyAdmin);
        }

        [TestMethod]
        public void ModifyBuildingManagerCorrectTestLogic()
        {
            // Arrange
            User newManager = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                LastName = "Manager",
                Email = "daed@gak.com",
                Role = Roles.Manager
            };

            User constructionCompanyAdmin = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                LastName = "ConstructionCompAdmin",
                Email = "daeda@gmail.com",
                Role = Roles.ConstructionCompanyAdmin
            };

            ConstructionCompany constructionCompany = new ConstructionCompany()
            {
                Name = "Construction Company",
                ConstructionCompanyAdmin = constructionCompanyAdmin
            };

            Building building = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = constructionCompany,
                ConstructionCompanyAdmin = constructionCompanyAdmin,
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "aeda@gmail.com"}
                    }
                }
            };

            Building expectedBuilding = new Building()
            {
                Id = Guid.NewGuid(),
                Name = "Building 1",
                Address = "Address 1",
                ConstructionCompany = constructionCompany,
                ConstructionCompanyAdmin = constructionCompanyAdmin,
                CommonExpenses = 100,
                Apartments = new List<Apartment>
                {
                    new Apartment()
                    {
                        Floor = 1,
                        Number = 101,
                        Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "aeda@gmail.com"}
                    }
                },
                managerId = newManager.Id
            };

            Mock<IBuildingRepository> buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
            Mock<IServiceRequestRepository> serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            userRepo.Setup(l => l.GetUserById(It.IsAny<Guid>())).Returns(newManager);
            buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(building);
            buildingRepo.Setup(l => l.UpdateBuilding(It.IsAny<Building>()));
            buildingRepo.Setup(l => l.Save());

            BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

            //Act
            Building logicResult = buildingLogic.ModifyBuildingManager(building.Id, newManager.Id, constructionCompanyAdmin.Id);

            //Assert
            buildingRepo.VerifyAll();
            userRepo.VerifyAll();
            constructionCompanyRepo.VerifyAll();
            serviceRequestRepo.VerifyAll();
            Assert.AreEqual(logicResult, expectedBuilding);
        }

        [TestMethod]
        public void ModifyBuildingManagerIncorrectAdminDoesNotOwnBuildingTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
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

                User contructionCompanyAdminNotAllowed = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    LastName = "ConstructionCompAdmin",
                    Email = "cdcs@gmial.com",
                    Role = Roles.ConstructionCompanyAdmin,
                };

                User manager = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Manager",
                    LastName = "Manager",
                    Email = "scs@gmail.com",
                    Role = Roles.Manager
                };

                ConstructionCompany constructionCompany = new ConstructionCompany()
                {
                    Name = "Construction Company",
                    ConstructionCompanyAdmin = constructionComAdmin
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Building 1",
                    Address = "Address 1",
                    ConstructionCompany = constructionCompany,
                    ConstructionCompanyAdmin = constructionComAdmin,
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>
                    {
                        new Apartment()
                        {
                            Floor = 1,
                            Number = 101,
                            Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "aeda@gmail.com"}
                        }
                    }
                };

                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                userRepo.Setup(l => l.GetUserById(It.IsAny<Guid>())).Returns(manager);
                buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(expectedBuilding);


                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.ModifyBuildingManager(expectedBuilding.Id, Guid.NewGuid(), contructionCompanyAdminNotAllowed.Id);

            }
            catch (InvalidOperationException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(InvalidOperationException));
            Assert.IsTrue(specificEx.Message.Contains("Construction company admin is not the owner of the building"));
        }

        [TestMethod]
        public void ModifyBuildingManagerIncorrectNewManagerIsAlreadyManagerOfTheBuildingTestLogic()
        {
            // Arrange
            Exception specificEx = null;
            Mock<IBuildingRepository> buildingRepo = null;
            Mock<IServiceRequestRepository> serviceRequestRepo = null;
            Mock<IConstructionCompanyRepository> constructionCompanyRepo = null;
            Mock<IUserRepository> userRepo = null;
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

                User contructionCompanyAdminNotAllowed = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    LastName = "ConstructionCompAdmin",
                    Email = "cdcs@gmial.com",
                    Role = Roles.ConstructionCompanyAdmin,
                };

                User manager = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Manager",
                    LastName = "Manager",
                    Email = "scs@gmail.com",
                    Role = Roles.Manager
                };

                ConstructionCompany constructionCompany = new ConstructionCompany()
                {
                    Name = "Construction Company",
                    ConstructionCompanyAdmin = constructionComAdmin
                };

                Building expectedBuilding = new Building()
                {
                    Id = Guid.NewGuid(),
                    Name = "Building 1",
                    Address = "Address 1",
                    ConstructionCompany = constructionCompany,
                    ConstructionCompanyAdmin = constructionComAdmin,
                    CommonExpenses = 100,
                    Apartments = new List<Apartment>
                    {
                        new Apartment()
                        {
                            Floor = 1,
                            Number = 101,
                            Owner = new Owner { Name = "Jane", LastName = "Doe", Email = "aeda@gmail.com"}
                        }
                    },
                    managerId = manager.Id
                };

                buildingRepo = new Mock<IBuildingRepository>(MockBehavior.Strict);
                serviceRequestRepo = new Mock<IServiceRequestRepository>(MockBehavior.Strict);
                constructionCompanyRepo = new Mock<IConstructionCompanyRepository>(MockBehavior.Strict);
                userRepo = new Mock<IUserRepository>(MockBehavior.Strict);

                userRepo.Setup(l => l.GetUserById(It.IsAny<Guid>())).Returns(manager);
                buildingRepo.Setup(l => l.GetBuildingById(It.IsAny<Guid>())).Returns(expectedBuilding);


                BuildingLogic buildingLogic = new BuildingLogic(buildingRepo.Object, serviceRequestRepo.Object, constructionCompanyRepo.Object, userRepo.Object);

                // Act
                Building logicResult = buildingLogic.ModifyBuildingManager(expectedBuilding.Id, manager.Id, contructionCompanyAdminNotAllowed.Id);

            }
            catch (InvalidOperationException e)
            {
                specificEx = e;
            }

            // Assert
            buildingRepo.VerifyAll();
            Assert.IsNotNull(specificEx);
            Assert.IsInstanceOfType(specificEx, typeof(InvalidOperationException));
            Assert.IsTrue(specificEx.Message.Contains("New manager is already the manager of the building"));
        }
    }
}
