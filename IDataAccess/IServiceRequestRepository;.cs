using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IServiceRequestRepository
    {
        ServiceRequest CreateServiceRequest(ServiceRequest serviceRequest);
        IEnumerable<ServiceRequest> GetAllServiceRequestsManager(string category, Guid managerId);
        IEnumerable<ServiceRequest> GetAllServiceRequestsByMaintenanceUserId(Guid maintenanceUserId);
        IEnumerable<ServiceRequest> GetAllServiceRequests();
        ServiceRequest GetServiceRequestById(Guid serviceRequestId);
        void UpdateServiceRequest(ServiceRequest serviceRequest);
        List<ServiceRequest> GetServiceRequestsByBuilding(Guid buildingId);
        IEnumerable<ServiceRequest> GetServiceRequestsByBuildingId(Guid id); //las no cerradas se cambiaron a genericas
    }
}
