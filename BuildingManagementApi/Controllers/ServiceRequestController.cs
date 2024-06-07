using BuildingManagementApi.Filters;
using BusinessLogic;
using Domain;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/serviceRequests")]
    [ExceptionFilter]
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
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult CreateServiceRequest([FromBody] ServiceRequestRequest serviceRequestToCreate)
        {
            string token = Request.Headers["Authorization"].ToString();
            var managerUser = _sessionService.GetUserByToken(Guid.Parse(token));

            var createdServiceRequest = _serviceRequestLogic.CreateServiceRequest(serviceRequestToCreate.ApartmentId, serviceRequestToCreate.CategoryId, serviceRequestToCreate.Description, managerUser);
            ServiceRequestResponse response = new ServiceRequestResponse(createdServiceRequest);

            return CreatedAtAction(nameof(CreateServiceRequest), new { id = response.Id }, response);
        }

        [HttpGet("manager-requests")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult GetAllServiceRequestsManager([FromQuery] string? category)
        {
            //Agregar id a partir del token para que solo vea las que el asigno
            string token = Request.Headers["Authorization"].ToString();
            var maintenanceUser = _sessionService.GetUserByToken(Guid.Parse(token));

            string categoryOrDefault = category == null ? "" : category;
            IEnumerable<ServiceRequestResponse> serviceRequests = _serviceRequestLogic.GetAllServiceRequestsManager(categoryOrDefault, maintenanceUser.Id).Select(sr => new ServiceRequestResponse(sr)).ToList();
            return Ok(serviceRequests);
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Maintenance)]
        public IActionResult GetAllServiceRequestsMaintenance()
        {
            string token = Request.Headers["Authorization"].ToString();
            var maintenanceUser = _sessionService.GetUserByToken(Guid.Parse(token));
            IEnumerable<ServiceRequestResponse> serviceRequests = _serviceRequestLogic.GetAllServiceRequestsMaintenance(maintenanceUser.Id).Select(sr => new ServiceRequestResponse(sr)).ToList();
            return Ok(serviceRequests);
        }

        [HttpPatch("{id}/assign-request")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult AssignRequestToMaintainancePerson([FromRoute] Guid id, [FromBody] IdRequest maintainancePersonId)
        {
            var serviceRequest = _serviceRequestLogic.AssignRequestToMaintainancePerson(id, maintainancePersonId.Id);
            ServiceRequestResponse response = new ServiceRequestResponse(serviceRequest);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Maintenance)]
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
