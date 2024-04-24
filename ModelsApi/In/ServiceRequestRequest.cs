using Domain;
using Domain.@enum;

namespace ModelsApi.In
{
    public class ServiceRequestRequest
    {
        public string Description { get; set; }
        public ApartmentRequest Apartment { get; set; }
        public CategoryRequest Category { get; set; }
        public ServiceRequestStatus Status { get; set; }

        public ServiceRequest ToEntity()
        {
            var serviceRequest = new ServiceRequest
            {
                Description = Description,
                Apartment = Apartment.ToEntity(),
                Category = Category.ToEntity(),
                Status = Status
            };

            return serviceRequest;
        }
    }
}