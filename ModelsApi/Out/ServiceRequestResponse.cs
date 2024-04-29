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
        public Guid Apartment { get; set; }
        public Guid Category { get; set; }
        public ServiceRequestStatus Status { get; set; }

        public ServiceRequestResponse (ServiceRequest expectedServiceRequest)
        {
            Id = expectedServiceRequest.Id;
            Description = expectedServiceRequest.Description;
            Apartment = expectedServiceRequest.Apartment;
            Category = expectedServiceRequest.Category;
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
