﻿using System;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class AdministratorResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public AdministratorResponse(User admin)
        {
            Id = admin.Id;
            Name = admin.Name;
            LastName = admin.LastName;
            Email = admin.Email;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            AdministratorResponse other = (AdministratorResponse)obj;
            return Id == other.Id && Name == other.Name && LastName == other.LastName && Email == other.Email;
        }
    }
}

