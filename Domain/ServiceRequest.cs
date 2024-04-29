using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ServiceRequest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid Apartment { get; set; }
        public Guid Category { get; set; }
        public ServiceRequestStatus Status { get; set; }

        public Guid? MaintainancePersonId { get; set; }

        public ServiceRequest() { }
    }
}
