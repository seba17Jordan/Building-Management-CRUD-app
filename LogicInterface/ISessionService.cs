using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface ISessionService
    {
        Guid Authenticate(string email, string password);
        User GetUserByToken(Guid token);
    }
}
