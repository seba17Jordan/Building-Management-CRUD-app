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
        private readonly IUserRepository _userRepository;

        public InvitationLogic(IInvitationRepository invitationRepository, IUserRepository userRepository)
        {
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
        }

        public Invitation GetInvitationById(Guid id)
        {
            return _invitationRepository.GetInvitationById(id);
        }

        /*
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
        */

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
            Invitation invitation = _invitationRepository.GetInvitationById(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid id");
            }

            if (invitation == null)
            {
                throw new ArgumentException("Invitation not found");
            }

            if(invitation.State != Status.Rejected)
            {
                throw new InvalidOperationException("You can only delete a rejected invitation");
            }
            _invitationRepository.DeleteInvitation(id);
        }

        public void RejectInvitation(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid id");
            }

            Invitation invitation = _invitationRepository.GetInvitationById(id);

            if (invitation == null)
            {
                throw new ArgumentException("Invitation not found");
            }

            if (invitation.State != Status.Pending)
            {
                throw new InvalidOperationException("You can only reject a pending invitation");
            }

            invitation.State = Status.Rejected;
            _invitationRepository.UpdateInvitation(invitation);
        }

        public User AcceptInvitation(Guid guid, User managerToCreate)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Invalid id");
            }

            Invitation invitation = _invitationRepository.GetInvitationById(guid);

            if (invitation == null)
            {
                throw new ArgumentException("Invitation not found");
            }

            if (invitation.State != Status.Pending)
            {
                throw new InvalidOperationException("You can only accept a pending invitation");
            }

            if(invitation.Email != managerToCreate.Email)
            {
                throw new InvalidOperationException("The email of the invitation and the email of the user do not match");
            }

            invitation.State = Status.Accepted;
            _invitationRepository.UpdateInvitation(invitation);

            //Ahora creo el manager
            managerToCreate.Name = invitation.Name;
            managerToCreate.Role = Roles.Manager;
            return _userRepository.CreateUser(managerToCreate);
        }
    }
}
