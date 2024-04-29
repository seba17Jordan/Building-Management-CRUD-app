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
    }
}
