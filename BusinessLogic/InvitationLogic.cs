﻿using Domain;
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

            managerToCreate.Name = invitation.Name;
            managerToCreate.Role = Roles.Manager;
            managerToCreate.LastName = "";

            return _userRepository.CreateUser(managerToCreate);
        }
    }
}
