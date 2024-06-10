using Domain;
using Domain.@enum;

namespace ModelsApi.In
{
    public class ServiceRequestRequest
    {
        public string Description { get; set; }
        public Guid ApartmentId { get; set; }
        public Guid CategoryId { get; set; }
        public ServiceRequestStatus Status { get; set; }

        public ServiceRequest ToEntity()
        {
            ServiceRequest serviceRequest = new ServiceRequest
            {
                Description = Description,
                Status = Status
            };

            return serviceRequest;
        }
    }
}