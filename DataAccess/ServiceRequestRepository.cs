using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly DbContext _context;

        public ServiceRequestRepository(DbContext context)
        {
            _context = context;
        }

        public ServiceRequest CreateServiceRequest(ServiceRequest serviceRequest)
        {
            _context.Set<ServiceRequest>().Add(serviceRequest);
            _context.SaveChanges();
            return serviceRequest;
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequestsManager(string categoryName, Guid managerId)
        {
            return _context.Set<ServiceRequest>().Where(sr => (categoryName == "" || sr.CategoryName == categoryName) && (sr.ManagerId == managerId)).ToList();
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequestsByMaintenanceUserId(Guid maintenanceUserId)
        {
            return _context.Set<ServiceRequest>().Where(sr => sr.MaintainancePersonId == maintenanceUserId).ToList();
        }

        public ServiceRequest GetServiceRequestById(Guid serviceRequestId)
        {
            return _context.Set<ServiceRequest>().Find(serviceRequestId);
        }

        public bool ServiceRequestExists(Guid id)
        {
            return _context.Set<ServiceRequest>().Any(sr => sr.Id == id);
        }

        public void UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            _context.Set<ServiceRequest>().Update(serviceRequest);
            _context.SaveChanges();
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequests()
        {
            return _context.Set<ServiceRequest>().ToList();
        }

        public List<ServiceRequest> GetServiceRequestsByBuilding(Guid buildingId)
        {
            return _context.Set<ServiceRequest>().Where(sr => sr.BuildingId == buildingId).ToList();
        }

        public IEnumerable<ServiceRequest> GetNoClosedServiceRequestsByBuildingId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
