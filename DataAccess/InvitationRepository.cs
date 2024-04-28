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
            throw new System.NotImplementedException();
        }

        public bool InvitationExists(Func<Invitation, bool> func)
        {
            return _context.Set<Invitation>().Any(func);
        }
    }
}
