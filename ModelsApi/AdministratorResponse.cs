using System;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi
{
    public class AdministratorResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public AdministratorResponse(Administrator admin)
        {
            Id = admin.Id;
            Name = admin.Name;
            LastName = admin.Lastname;
            Email = admin.Email;
        }
    }
}

