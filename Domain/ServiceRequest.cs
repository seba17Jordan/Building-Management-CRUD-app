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
        public string CategoryName { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public Guid BuildingId { get; set; }
        public Guid? MaintainancePersonId { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ServiceRequest() { }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ServiceRequest serviceRequest = (ServiceRequest)obj;
            return Id == serviceRequest.Id &&
                Description == serviceRequest.Description &&
                Apartment == serviceRequest.Apartment &&
                Category == serviceRequest.Category &&
                Status == serviceRequest.Status &&
                MaintainancePersonId == serviceRequest.MaintainancePersonId;
        }
    }
}
