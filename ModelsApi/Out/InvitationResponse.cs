using Domain;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class InvitationResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid Id { get; set; }
        public Status State { get; set; }
        public Roles Role { get; set; }

        public InvitationResponse(Invitation invitation)
        {
            Id = invitation.Id;
            Email = invitation.Email;
            Name = invitation.Name;
            ExpirationDate = invitation.ExpirationDate;
            State = invitation.State;
            Role = invitation.Role;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            InvitationResponse other = (InvitationResponse)obj;
            return Id.Equals(other.Id) &&
                   Email.Equals(other.Email) &&
                   Name.Equals(other.Name) &&
                   ExpirationDate.Equals(other.ExpirationDate) &&
                   State.Equals(other.State);
        }
    }
}
