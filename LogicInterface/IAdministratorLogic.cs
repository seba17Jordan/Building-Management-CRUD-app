using System;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IAdministratorLogic
    {
        Administrator CreateAdministrator(Administrator admin);
    }
}
