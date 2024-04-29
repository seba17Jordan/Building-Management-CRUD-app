using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;

namespace BusinessLogic
{
    public class RequestLogic : IServiceRequestLogic
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        public RequestLogic(IServiceRequestRepository serviceRequestRepository)
        {
            _serviceRequestRepository = serviceRequestRepository;
        }
        public ServiceRequest CreateServiceRequest(ServiceRequest serviceRequest)
        {
            if (serviceRequest == null)
            {
                throw new ArgumentNullException(nameof(serviceRequest), "Service request is null");
            }
            if (string.IsNullOrWhiteSpace(serviceRequest.Description)) { 
                throw new ArgumentException("Service request description is empty", nameof(serviceRequest.Description));
            }
            if(serviceRequest.Apartment == null)
            {
                throw new ArgumentException("Service request must have an apartment", nameof(serviceRequest.Apartment));
            }
            if(serviceRequest.Category == null)
            {
                throw new ArgumentException("Service request must have a category", nameof(serviceRequest.Category));
            }
            return _serviceRequestRepository.CreateServiceRequest(serviceRequest);
        }

    }
}
