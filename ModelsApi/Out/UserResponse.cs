using Domain;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Roles Role { get; set; }

        public UserResponse (User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Role = user.Role;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var userResponse = obj as UserResponse;
            return Email == userResponse.Email
                && Name == userResponse.Name
                && Role == userResponse.Role;
        }
    }
}
