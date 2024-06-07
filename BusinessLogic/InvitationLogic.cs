using CustomExceptions;
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

        public Invitation CreateInvitation(Invitation invitation)
        {
            if (invitation == null)
            {
                throw new ArgumentNullException(nameof(invitation), "Invitation can't be null");
            }

            invitation.SelfValidate();

            if (_invitationRepository.InvitationExists(invitation.Email))
            {
                throw new ObjectAlreadyExistsException("Invitation with same Email already exists");
            }
            
            return _invitationRepository.CreateInvitation(invitation);
        }

        public void DeleteInvitation(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new EmptyFieldException("Invitation Id is Empty");
            }

            Invitation invitation = _invitationRepository.GetInvitationById(id);

            if (invitation == null)
            {
                throw new ObjectNotFoundException("Invitation not found");
            }

            if(invitation.State == Status.Accepted)
            {
                throw new InvalidOperationException("This invitation is already accepted");
            }

            _invitationRepository.DeleteInvitation(id);
        }

        public void RejectInvitation(string invitationEmail)
        {
            
            Invitation invitation = _invitationRepository.GetInvitationByMail(invitationEmail);

            if (invitation == null)
            {
                throw new ObjectNotFoundException("Invitation not found");
            }

            if (invitation.State != Status.Pending)
            {
                throw new InvalidOperationException("You can only reject a pending invitation");
            }

            invitation.State = Status.Rejected;
            _invitationRepository.UpdateInvitation(invitation);
        }

        public User AcceptInvitation(User userToCreate)
        {
            
            Invitation invitation = _invitationRepository.GetInvitationByMail(userToCreate.Email);

            if (invitation == null)
            {
                throw new ObjectNotFoundException("Invitation not found");
            }

            if(invitation.ExpirationDate < DateTime.Now)
            {
                throw new InvalidOperationException("The invitation has expired");
            }

            if (invitation.State != Status.Pending)
            {
                throw new InvalidOperationException("You can only accept a pending invitation");
            }

            if(invitation.Email != userToCreate.Email)
            {
                throw new InvalidOperationException("The email of the invitation and the email of the user do not match");
            }

            invitation.State = Status.Accepted;
            _invitationRepository.UpdateInvitation(invitation);

            userToCreate.Name = invitation.Name;
            userToCreate.Role = invitation.Role;
            userToCreate.LastName = "";
            userToCreate.SelfValidate();


            return _userRepository.CreateUser(userToCreate);
        }

        public IEnumerable<Invitation> GetAllInvitations()
        {
            throw new NotImplementedException();
        }
    }
}
