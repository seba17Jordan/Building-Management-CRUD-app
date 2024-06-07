using Domain;
using Domain.@enum;

namespace ModelsApi.In
{
    public class ServiceRequestRequest
    {
        public string Description { get; set; }
        public Guid Apartment { get; set; }
        public Guid Category { get; set; }
        public ServiceRequestStatus Status { get; set; }

        public ServiceRequest ToEntity()
        {
            ServiceRequest serviceRequest = new ServiceRequest
            {
                Description = Description,
                ApartmentId = Apartment,
                CategoryId = Category,
                Status = Status
            };

            return serviceRequest;
        }
    }
}