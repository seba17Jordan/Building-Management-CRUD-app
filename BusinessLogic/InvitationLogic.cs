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

        public Invitation GetInvitationById(Guid id)
        {
            return _invitationRepository.GetInvitationById(id);
        }

        public Invitation UpdateInvitationState(Guid id, Status status)
        {
            Invitation invitation = GetInvitationById(id);
            invitation.State = status;
            return _invitationRepository.UpdateInvitation(invitation);
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
