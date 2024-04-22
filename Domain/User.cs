using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public User() { 
            Id = Guid.NewGuid();
        }

        public User(string name, string email, string password) { 
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
