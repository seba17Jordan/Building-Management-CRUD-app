using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;

namespace BusinessLogic
{
    public class RequestLogic : IServiceRequestLogic
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICategoryRepository _categoryRepository;
        public RequestLogic(IServiceRequestRepository serviceRequestRepository, IBuildingRepository buildingRepository, ICategoryRepository categoryRepository)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _buildingRepository = buildingRepository;
            _categoryRepository = categoryRepository;
        }

        public ServiceRequest AssignRequestToMaintainancePerson(Guid serviceRequestId, Guid maintainancePersonId)
        {
            throw new NotImplementedException();
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
            if (!_buildingRepository.ExistApartment(serviceRequest.Apartment)) { 
                throw new ArgumentException("Apartment does not exist", nameof(serviceRequest.Apartment));
            }
            if (!_categoryRepository.FindCategoryById(serviceRequest.Category)) { 
                throw new ArgumentException("Category does not exist", nameof(serviceRequest.Category));
            }
            serviceRequest.Status = Domain.@enum.ServiceRequestStatus.Open;
            return _serviceRequestRepository.CreateServiceRequest(serviceRequest);
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequests(string category)
        {
            throw new NotImplementedException();
        }
    }
}
