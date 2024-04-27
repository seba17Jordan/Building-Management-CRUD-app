using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;

namespace BusinessLogic
{
    public class InvitationLogic : IInvitationLogic
    {
        private readonly IInvitationRepository _invitationRepository;
        public InvitationLogic(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public User AcceptInvitation(Guid id, string email, string password)
        {
            throw new NotImplementedException();
        }

        public Invitation CreateInvitation(Invitation invitation)
        {
            throw new NotImplementedException();
        }

        public void DeleteInvitation(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RejectInvitation(Guid id)
        {
            throw new NotImplementedException();
        }

        private Func<Invitation, bool> GetInvitationsByEmail(string email)
        {
            return (Invitation i) => email == "" || i.Email == email;
        }
    }
}
