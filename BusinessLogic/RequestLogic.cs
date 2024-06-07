using CustomExceptions;
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
            if(maintainancePersonId == Guid.Empty || serviceRequestId == Guid.Empty)
            {
                throw new EmptyFieldException("Missing Id");
            }

            User maintainancePerson = _userRepository.GetUserById(maintainancePersonId);

            if(maintainancePerson == null)
            {
                throw new ObjectNotFoundException("Maintainance person does not exist");
            }

            if(maintainancePerson.Role != Roles.Maintenance)
            {
                throw new ArgumentException("User is not a maintainance person");
            }

            ServiceRequest serviceRequest = _serviceRequestRepository.GetServiceRequestById(serviceRequestId);

            if(serviceRequest == null)
            {
                throw new ObjectNotFoundException("Service request does not exist");
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

            serviceRequest.SelfValidate();
            
            if (!_buildingRepository.ExistApartment(serviceRequest.ApartmentId)) { 
                throw new ObjectNotFoundException("Apartment does not exist");
            }
            if (!_categoryRepository.FindCategoryById(serviceRequest.CategoryId)) { 
                throw new ObjectNotFoundException("Category does not exist");
            }

            //Documentar
            Category cat = _categoryRepository.GetCategoryById(serviceRequest.CategoryId);
            serviceRequest.Category = cat;
            serviceRequest.CategoryId = cat.Id;

            Apartment apartment = _buildingRepository.GetApartmentById(serviceRequest.ApartmentId);
            serviceRequest.Apartment = apartment;
            serviceRequest.ApartmentId = apartment.Id;

            Guid buildingId = _buildingRepository.GetBuildingIdByApartmentId(apartment);
            Building building = _buildingRepository.GetBuildingById(buildingId);
            serviceRequest.BuildingId = building.Id;
            serviceRequest.Building = building;

            serviceRequest.Status = ServiceRequestStatus.Open;
            return _serviceRequestRepository.CreateServiceRequest(serviceRequest);
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequestsManager(string category, Guid managerId)
        {
            return _serviceRequestRepository.GetAllServiceRequestsManager(category, managerId);
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequestsMaintenance(Guid maintenanceUserId)
        {
            return _serviceRequestRepository.GetAllServiceRequestsByMaintenanceUserId(maintenanceUserId);
        }

        public ServiceRequest UpdateServiceRequestStatus(Guid id, Guid maintenanceUserId, decimal? totalCost)
        {
            ServiceRequest serviceRequest = _serviceRequestRepository.GetServiceRequestById(id);
            
            if (serviceRequest == null)
            {
                throw new ObjectNotFoundException("Service request does not exist");
            }

            //Solo puedo aceptar o cerrar las solicitudes asignadas para mi
            if(serviceRequest.MaintainancePersonId != maintenanceUserId)
            {
                throw new ArgumentException("Service request is not assigned to the current maintainance person", nameof(serviceRequest));
            }

            if (totalCost != null) //quiere cerrar
            {
                if (serviceRequest.Status != ServiceRequestStatus.Attending)
                {
                    throw new InvalidOperationException("Service request is not attending");
                }

                if (totalCost < 0)
                {
                    throw new ArgumentException("Total cost is negative");
                }
                serviceRequest.TotalCost = totalCost;
                serviceRequest.Status = ServiceRequestStatus.Closed;
                serviceRequest.EndDate = DateTime.Now;
            }
            else //quiere aceptar
            { 
                if (serviceRequest.Status != ServiceRequestStatus.Open)
                {
                    throw new InvalidOperationException("Service request is not open");
                }
                serviceRequest.Status = ServiceRequestStatus.Attending;
                serviceRequest.StartDate = DateTime.Now;
            }
            _serviceRequestRepository.UpdateServiceRequest(serviceRequest);
            return serviceRequest;
        }
    }
}
