using Domain;
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

        public bool InvitationExists(Func<Invitation, bool> func)
        {
            return _context.Set<Invitation>().Any(func);
        }
    }
}
