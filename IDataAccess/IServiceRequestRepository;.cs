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
        IEnumerable<ServiceRequest> GetAllServiceRequests(string category);
        IEnumerable<ServiceRequest> GetAllServiceRequestsByUserId(Guid maintenanceUserId);
        ServiceRequest GetServiceRequestById(Guid serviceRequestId);
        void UpdateServiceRequest(ServiceRequest serviceRequest);
    }
}
