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
        public Apartment Apartment { get; set; }
        public Category Category { get; set; }
        public ServiceRequestStatus Status { get; set; }

        public ServiceRequest() { }
    }
}
