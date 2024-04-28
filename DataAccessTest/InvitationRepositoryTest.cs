﻿using DataAccess;
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
                Name = "John Doe",
                Email = "johndoe@example.com",
                ExpirationDate = DateTime.Now.AddDays(6)
            };

            var context = CreateDbContext("UpdateInvitationCorrectTest");
            var invitationRepository = new InvitationRepository(context);

            // Act
            Invitation createdInvitation = invitationRepository.CreateInvitation(expectedInvitation);
            context.SaveChanges();

            createdInvitation.State = Status.Accepted;
            Invitation updatedInvitation = invitationRepository.UpdateInvitation(createdInvitation);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(Status.Accepted, updatedInvitation.State);
        }

        private DbContext CreateDbContext(string database)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(database).Options;
            return new Context(options);
        }
    }
}