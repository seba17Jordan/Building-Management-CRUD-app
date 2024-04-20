using Domain;
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
    }
}
