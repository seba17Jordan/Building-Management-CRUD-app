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
    }
}
