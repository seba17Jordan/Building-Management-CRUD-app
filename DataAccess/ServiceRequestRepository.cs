using Domain;
using Domain.@enum;
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
            return _context.Set<ServiceRequest>()
                .Where(sr => (categoryName == "" || (sr.Category != null && sr.Category.Name == categoryName)) && (sr.Manager != null && sr.Manager.Id == managerId))
                .Include(c => c.Category)
                .Include(m => m.MaintenancePerson)
                .Include(a => a.Apartment)
                .ThenInclude(o => o.Owner)
                .ToList();
        }

        public IEnumerable<ServiceRequest> GetAllServiceRequestsByMaintenanceUserId(Guid maintenanceUserId)
        {
            return _context.Set<ServiceRequest>()
                .Where(sr => sr.MaintenanceId == maintenanceUserId)
                .Include(c => c.Category)
                .Include(m => m.Manager)
                .Include(a => a.Apartment)
                .ThenInclude(o => o.Owner)
                .ToList();
        }

        public ServiceRequest GetServiceRequestById(Guid serviceRequestId)
        {
            return _context.Set<ServiceRequest>()
                .Include(c => c.Category)
                .Include(m => m.Manager)
                .Include(a => a.Apartment)
                .ThenInclude(o => o.Owner)
                .FirstOrDefault(sr => sr.Id == serviceRequestId);
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
            return _context.Set<ServiceRequest>().Where(sr => sr.Building.Id == buildingId).ToList();
        }

        public IEnumerable<ServiceRequest> GetNoClosedServiceRequestsByBuildingId(Guid id)
        {
            return _context.Set<ServiceRequest>().Where(sr => sr.Building.Id == id && sr.Status != ServiceRequestStatus.Closed).ToList();
        }
    }
}
