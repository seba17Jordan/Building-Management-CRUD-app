using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid Token { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Session()
        {
            Token = Guid.NewGuid();
        }
    }
}
