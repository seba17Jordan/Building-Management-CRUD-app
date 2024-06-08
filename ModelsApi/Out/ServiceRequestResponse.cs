using Domain;
using Domain.@enum;
using ModelsApi.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelsApi.Out
{
    public class ServiceRequestResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ApartmentResponse Apartment { get; set; }
        public CategoryResponse Category { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public UserResponse MaintenancePerson { get; set; }
        public UserResponse Manager { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalCost { get; set; }

        public ServiceRequestResponse (ServiceRequest expectedServiceRequest)
        {
            Id = expectedServiceRequest.Id;
            Description = expectedServiceRequest.Description;
            if (expectedServiceRequest.Apartment != null) Apartment = new ApartmentResponse(expectedServiceRequest.Apartment);
            if (expectedServiceRequest.Category != null) Category = new CategoryResponse(expectedServiceRequest.Category);
            if (expectedServiceRequest.MaintenancePerson != null) MaintenancePerson = new UserResponse(expectedServiceRequest.MaintenancePerson);
            if (expectedServiceRequest.Manager != null) Manager = new UserResponse(expectedServiceRequest.Manager);
            if (expectedServiceRequest.StartDate != null) StartDate = expectedServiceRequest.StartDate;
            if (expectedServiceRequest.EndDate != null) EndDate = expectedServiceRequest.EndDate;
            if (expectedServiceRequest.TotalCost != null) TotalCost = expectedServiceRequest.TotalCost;
            Status = expectedServiceRequest.Status;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ServiceRequestResponse objResponse = (ServiceRequestResponse)obj;
            return 
                Id == objResponse.Id &&
                Description == objResponse.Description &&
                Apartment.Equals(objResponse.Apartment) &&
                Category.Equals(objResponse.Category) &&
                Status == objResponse.Status;
        }
    }
}
