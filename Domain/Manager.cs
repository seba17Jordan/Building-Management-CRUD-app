using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Manager : User
    {
        public Manager() { }
        public Manager(string name, string email, string password) : base(name, email, password)
        {
        }
    }
}
