using BuildingManagementApi.Filters;
using BusinessLogic;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/serviceRequest")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestLogic _serviceRequestLogic;
        private readonly ISessionService _sessionService;

        public ServiceRequestController(IServiceRequestLogic serviceRequestLogic, ISessionService sessionService)
        {
            _serviceRequestLogic = serviceRequestLogic;
            _sessionService = sessionService;
        }

        [HttpPost]
        //[AuthenticationFilter([Roles.Manager])]
        public IActionResult CreateServiceRequest([FromBody] ServiceRequestRequest serviceRequestToCreate)
        {
            var serviceRequest = serviceRequestToCreate.ToEntity();
            var createdServiceRequest = _serviceRequestLogic.CreateServiceRequest(serviceRequest);
            ServiceRequestResponse response = new ServiceRequestResponse(createdServiceRequest);

            return CreatedAtAction(nameof(CreateServiceRequest), new { id = response.Id }, response);
        }

        [HttpGet]
        //[AuthenticationFilter([Roles.Manager])]
        public IActionResult GetAllServiceRequests([FromQuery] string? category)
        {
            string categoryOrDefault = category == null ? "" : category;
            IEnumerable<ServiceRequestResponse> serviceRequests = _serviceRequestLogic.GetAllServiceRequests(categoryOrDefault).Select(sr => new ServiceRequestResponse(sr)).ToList();
            return Ok(serviceRequests);
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        //[AuthenticationFilter([Roles.Manager])]
        public IActionResult AssignRequestToMaintainancePerson([FromRoute] Guid id, [FromBody] IdRequest maintainancePersonId)
        {

            var serviceRequest = _serviceRequestLogic.AssignRequestToMaintainancePerson(id, maintainancePersonId.Id);
            ServiceRequestResponse response = new ServiceRequestResponse(serviceRequest);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        //[AuthenticationFilter([Roles.Manager])]
        public IActionResult UpdateServiceRequestStatus([FromRoute] Guid id, [FromBody] UpdateServiceRequestStatusRequest updateServiceRequestStatusRequest)
        {
            //Para asegurar que quien actualiza el estado de la solicitud sea el mismo usuario de mantenimiento que la tiene asignada
            string token = Request.Headers["Authorization"].ToString();
            var maintenanceUser = _sessionService.GetUserByToken(Guid.Parse(token));

            var serviceRequest = _serviceRequestLogic.UpdateServiceRequestStatus(id, maintenanceUser.Id, updateServiceRequestStatusRequest.TotalCost);
            ServiceRequestResponse response = new ServiceRequestResponse(serviceRequest);
            return Ok(response);
        }
    }
}
