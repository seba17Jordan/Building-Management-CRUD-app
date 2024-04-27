using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Invitation { 
        public Status State { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Invitation() { }

        public Invitation(string email, string name, DateTime expirationDate)
        {
            Email = email;
            Name = name;
            ExpirationDate = expirationDate;
            State = Status.Pending;
        }
    }
}
