﻿using Domain;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class InvitationRequest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public Guid? id { get; set; }
        public Status State { get; set; }
        public string? NewPassword { get; set; }
        public Roles Role { get; set; }
        public InvitationRequest()
        {
            State = Status.Pending;
        }

        public Invitation ToEntity() {
            return new Invitation()
            {
                Email = Email,
                Name = Name,
                ExpirationDate = (DateTime)ExpirationDate,
                State = State,
                Role = Role
            };
        }
    }
}
