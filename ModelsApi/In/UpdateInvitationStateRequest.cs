using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class UpdateInvitationStateRequest
    {
        public Status Status { get; set; }
    }
}
