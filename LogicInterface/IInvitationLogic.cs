using Domain;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IInvitationLogic
    {
        Invitation CreateInvitation(Invitation invitation);
        void RejectInvitation(Guid id);
        void DeleteInvitation(Guid id);
        User AcceptInvitation(User managerToCreate);
    }
}
