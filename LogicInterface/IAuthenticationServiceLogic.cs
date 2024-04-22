using System;
using Domain.@enum;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IAuthenticationServiceLogic
    {
        public Roles GetUserRole(Guid token);
    }
}
