using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IServiceRequestLogic
    {
        ServiceRequest AssignRequestToMaintainancePerson(Guid serviceRequestId, Guid maintainancePersonId);
        ServiceRequest CreateServiceRequest(ServiceRequest serviceRequest);
        IEnumerable<ServiceRequest> GetAllServiceRequestsManager(string category, Guid mangerId);
        IEnumerable<ServiceRequest> GetAllServiceRequestsMaintenance(Guid maintenanceUserId);
        ServiceRequest UpdateServiceRequestStatus(Guid id, Guid maintenanceUserId, decimal? totalCost);
    }
}
