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
            if (invitation == null)
            {
                throw new ArgumentException("La invitación especificada no existe.");
            }
            if (invitation.ExpirationDate <= DateTime.Now)
            {
                throw new InvalidOperationException("La invitación ha expirado y no se puede actualizar su estado.");
            }
            if (invitation.State == Status.Expired)
            {
                throw new InvalidOperationException("La invitación ha expirado y no se puede actualizar su estado.");
            }

            if ((invitation.State == Status.Pending && (status != Status.Accepted && status != Status.Rejected))
                || (invitation.State == Status.Accepted && status != Status.Rejected)
                || (invitation.State == Status.Rejected && status != Status.Accepted))
            {
                throw new InvalidOperationException("No se puede cambiar la invitación a ese estado.");
            }
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
