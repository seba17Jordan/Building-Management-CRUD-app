using Domain;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class CreateInvitationResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid Id { get; set; }
        public Status State { get; set; }

        public CreateInvitationResponse(Invitation invitation)
        {
            Id = invitation.Id;
            Email = invitation.Email;
            Name = invitation.Name;
            ExpirationDate = invitation.ExpirationDate;
            State = invitation.State;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            CreateInvitationResponse other = (CreateInvitationResponse)obj;
            return Id.Equals(other.Id) &&
                   Email.Equals(other.Email) &&
                   Name.Equals(other.Name) &&
                   ExpirationDate.Equals(other.ExpirationDate) &&
                   State.Equals(other.State);
        }
    }
}
