using BuildingManagementApi.Filters;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestLogic _serviceRequestLogic;

        public ServiceRequestController(IServiceRequestLogic serviceRequestLogic)
        {
            _serviceRequestLogic = serviceRequestLogic;
        }

        [HttpPost]
        [AuthenticationFilter([Roles.Manager])]
        public IActionResult CreateServiceRequest([FromBody] ServiceRequestRequest serviceRequestToCreate)
        {
            var serviceRequest = serviceRequestToCreate.ToEntity();
            var createdServiceRequest = _serviceRequestLogic.CreateServiceRequest(serviceRequest);
            ServiceRequestResponse response = new ServiceRequestResponse(createdServiceRequest);

            return CreatedAtAction(nameof(CreateServiceRequest), new { id = response.Id }, response);
        }
    }
}
