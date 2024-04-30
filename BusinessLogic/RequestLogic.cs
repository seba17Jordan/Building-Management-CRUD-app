using Domain;
using Domain.@enum;
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
        private readonly IUserRepository _userRepository;
        public RequestLogic(IServiceRequestRepository serviceRequestRepository, IBuildingRepository buildingRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _buildingRepository = buildingRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public ServiceRequest AssignRequestToMaintainancePerson(Guid serviceRequestId, Guid maintainancePersonId)
        {
            if(maintainancePersonId == null || serviceRequestId == null)
            {
                throw new ArgumentNullException("id is null");
            }

            User maintainancePerson = _userRepository.GetUserById(maintainancePersonId);

            if(maintainancePerson == null)
            {
                throw new ArgumentException("Maintainance person does not exist", nameof(maintainancePerson));
            }

            if(maintainancePerson.Role != Roles.Maintenance)
            {
                throw new ArgumentException("User is not a maintainance person", nameof(maintainancePerson));
            }

            ServiceRequest serviceRequest = _serviceRequestRepository.GetServiceRequestById(serviceRequestId);

            if(serviceRequest == null)
            {
                throw new ArgumentException("Service request does not exist", nameof(serviceRequest));
            }

            if(serviceRequest.Status != ServiceRequestStatus.Open)
            {
                throw new ArgumentException("Service request is not open", nameof(serviceRequest));
            }

            serviceRequest.MaintainancePersonId = maintainancePersonId;
            _serviceRequestRepository.UpdateServiceRequest(serviceRequest);
            return serviceRequest;
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

            //Documentar
            Category cat = _categoryRepository.GetCategoryById(serviceRequest.Category);
            serviceRequest.CategoryName = cat.Name;
            Apartment apartment = _buildingRepository.GetApartmentById(serviceRequest.Apartment);
            serviceRequest.BuildingId = _buildingRepository.GetBuildingIdByApartmentId(apartment);
            serviceRequest.Status = ServiceRequestStatus.Open;
            return _serviceRequestRepository.CreateServiceRequest(serviceRequest);
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequests(string category)
        {
            return _serviceRequestRepository.GetAllServiceRequests(category);
        }

        public ServiceRequest UpdateServiceRequestStatus(Guid id, decimal? totalCost)
        {
            ServiceRequest serviceRequest = _serviceRequestRepository.GetServiceRequestById(id);
            
            if (serviceRequest == null)
            {
                throw new ArgumentException("Service request does not exist", nameof(serviceRequest));
            }

            //Recibir el id de la persona por parametro y validar que es la misma que tiene esta solicitud asignada
            /*
            if(serviceRequest.MaintainancePersonId != currentPersonId)
            {
                throw new ArgumentException("Service request is not assigned to the current maintainance person", nameof(serviceRequest));
            }
            */

            if (totalCost != null) //quiere cerrar
            {
                if (serviceRequest.Status != ServiceRequestStatus.Attending)
                {
                    throw new ArgumentException("Service request is not attending", nameof(serviceRequest));
                }

                if (totalCost < 0)
                {
                    throw new ArgumentException("Total cost is negative", nameof(totalCost));
                }
                serviceRequest.TotalCost = totalCost;
                serviceRequest.Status = ServiceRequestStatus.Closed;
                serviceRequest.EndDate = DateTime.Now;
            }
            else //quiere aceptar
            { 
                if (serviceRequest.Status != ServiceRequestStatus.Open)
                {
                    throw new ArgumentException("Service request is not open", nameof(serviceRequest));
                }
                serviceRequest.Status = ServiceRequestStatus.Attending;
                serviceRequest.StartDate = DateTime.Now;
            }
            _serviceRequestRepository.UpdateServiceRequest(serviceRequest);
            return serviceRequest;
        }
    }
}
