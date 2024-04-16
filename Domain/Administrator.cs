using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Administrator
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }


        public Administrator()
        {
            Id = Guid.NewGuid();
        }

        public Administrator(string email, string password, string name, string lastname)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            Name = name;
            Lastname = lastname;
        }
    }
}
