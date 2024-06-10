using CustomExceptions;
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
        public Apartment? Apartment { get; set; }
        public Category? Category { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public Building? Building { get; set; }
        public User? Manager { get; set; }
        public User? MaintenancePerson { get; set; }
        public Guid? MaintenanceId { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ServiceRequest() { }

        public void SelfValidate()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new EmptyFieldException("There are empty fields");
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ServiceRequest serviceRequest = (ServiceRequest)obj;
            return 
                Description == serviceRequest.Description &&
                Apartment.Equals(serviceRequest.Apartment) &&
                Category.Equals(serviceRequest.Category) &&
                Status == serviceRequest.Status;
        }
    }
}
