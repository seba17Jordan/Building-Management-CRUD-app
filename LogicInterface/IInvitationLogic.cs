using Domain;
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

        Manager AcceptInvitation(Guid id, string email, string password);

        void RejectInvitation(Guid id);
        void DeleteInvitation(Guid id);
    }
}
