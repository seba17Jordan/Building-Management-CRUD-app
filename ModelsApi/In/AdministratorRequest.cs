using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class AdministratorRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }

        public Administrator ToEntity()
        {
            return new Administrator
            {
                Email = Email,
                Password = Password,
                Name = Name,
                Lastname = Lastname
            };
        }
    }
}

