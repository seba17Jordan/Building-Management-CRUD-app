using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }
    }
}
