﻿using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataAccess
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly DbContext _context;

        public InvitationRepository(DbContext context)
        {
            _context = context;
        }

        public Invitation CreateInvitation(Invitation invitation)
        {
            _context.Set<Invitation>().Add(invitation);
            _context.SaveChanges();
            return invitation;
        }

        public Invitation GetInvitationById(Guid id)
        {
            Invitation invitation = _context.Find<Invitation>(id);
            if (invitation == null)
            {
                throw new ArgumentException("Invitation not found");
            }
            return invitation;
        }

        public void UpdateInvitation(Invitation invitation)
        {
            _context.Set<Invitation>().Update(invitation);
            _context.SaveChanges();
        }

        public bool InvitationExists(string email)
        {
            return _context.Set<Invitation>().Any(i => i.Email == email);
        }

        public void DeleteInvitation(Guid id)
        {
            _context.Set<Invitation>().Remove(GetInvitationById(id));
            _context.SaveChanges();
        }

        public Invitation GetInvitationByMail(string email)
        {
            return _context.Set<Invitation>().FirstOrDefault(i => i.Email == email);
        }

        public IEnumerable<Invitation> GetAllInvitations()
        {
            return _context.Set<Invitation>().ToList();
        }
    }
}
