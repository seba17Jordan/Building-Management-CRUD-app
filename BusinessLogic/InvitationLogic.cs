using Domain;
using Domain.@enum;
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

        public Invitation UpdateInvitationState(Guid id, Status status)
        {
            throw new NotImplementedException();
        }

        public Invitation CreateInvitation(Invitation invitation)
        {
            if (invitation == null)
            {
                throw new ArgumentNullException(nameof(invitation), "Invitation can't be null");
            }

            if (_invitationRepository.InvitationExists(invitation.Email))
            {
                throw new ArgumentException("Invitation already exists");
            }

            if (string.IsNullOrWhiteSpace(invitation.Email) ||
                               string.IsNullOrWhiteSpace(invitation.Name))
            {
                throw new ArgumentException("Invalid data");
            }
            return _invitationRepository.CreateInvitation(invitation);
        }

        public void DeleteInvitation(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RejectInvitation(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
