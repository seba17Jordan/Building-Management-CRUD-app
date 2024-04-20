﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class AcceptInvitationResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Password { get; set; }

        public AcceptInvitationResponse(Manager manager) {
            Id = manager.Id;
            Name = manager.Name;
            Email = manager.Email;
            Password = manager.Password;
        }
    }
}
