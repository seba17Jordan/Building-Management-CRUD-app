using Domain;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class CreateInvitationRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid id { get; set; }
        public Status State { get; set; }

        public CreateInvitationRequest()
        {
            State = Status.Pending;
        }

        public Invitation ToEntity() {
            return new Invitation()
            {
                Id = id,
                Email = Email,
                Name = Name,
                ExpirationDate = ExpirationDate,
                State = State
            };
        }
    }
}
