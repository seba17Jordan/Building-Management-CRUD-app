using DataAccess;
using Domain;
using Domain.@enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessTest
{
    [TestClass]
    public class InvitationRepositoryTest
    {
        [TestMethod]
        public void CreateInvitationRepositoryCorrectTest()
        {
            var expectedInvitation = new Invitation()
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            var context = CreateDbContext("CreateInvitationCorrectTest");
            var invitationRepository = new InvitationRepository(context);

            // Act
            Invitation createdInvitation = invitationRepository.CreateInvitation(expectedInvitation);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedInvitation, createdInvitation);
        }

        [TestMethod]
        public void UpdateInvitationRepositoryCorrectTest()
        {
            var expectedInvitation = new Invitation()
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            var context = CreateDbContext("UpdateInvitationRepositoryCorrectTest");
            var invitationRepository = new InvitationRepository(context);

            // Act
            Invitation createdInvitation = invitationRepository.CreateInvitation(expectedInvitation);
            context.SaveChanges();

            createdInvitation.State = Status.Rejected;
            invitationRepository.UpdateInvitation(createdInvitation);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(Status.Rejected, createdInvitation.State);
        }

        [TestMethod]
        public void DeleteInvitationRepositoryCorrectTest()
        {
            var expectedInvitation = new Invitation()
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            var context = CreateDbContext("DeleteInvitationCorrectTest");
            var invitationRepository = new InvitationRepository(context);

            // Act
            Invitation createdInvitation = invitationRepository.CreateInvitation(expectedInvitation);
            context.SaveChanges();

            invitationRepository.DeleteInvitation(createdInvitation.Id);
            context.SaveChanges();

            // Assert
            Assert.IsFalse(context.Set<Invitation>().Any(i => i.Id == createdInvitation.Id));
        }

        [TestMethod]
        public void GetInvitationByMailRepositoryCorrectTest()
        {
            var expectedInvitation = new Invitation()
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            var context = CreateDbContext("GetInvitationByMailRepositoryCorrectTest");
            var invitationRepository = new InvitationRepository(context);

            // Act
            Invitation createdInvitation = invitationRepository.CreateInvitation(expectedInvitation);
            context.SaveChanges();

            invitationRepository.GetInvitationByMail(createdInvitation.Email);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expectedInvitation, createdInvitation);
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}