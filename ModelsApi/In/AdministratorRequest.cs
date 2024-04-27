using Domain;
using Domain.@enum;
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
        public string LastName { get; set; }
        public Roles Role { get; set; }

        public User ToEntity()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                throw new ArgumentException("There is a missing field in the request's body");
            }

            return new User
            {
                Email = Email,
                Password = Password,
                Name = Name,
                LastName = LastName,
                Role = Role
            };
        }
    }
}

