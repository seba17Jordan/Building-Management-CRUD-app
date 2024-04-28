using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Domain;
using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using BuildingManagementApi.Filters;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Domain.@enum;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/invitations")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationLogic _invitationLogic;

        public InvitationController(IInvitationLogic invitationLogic)
        {
            _invitationLogic = invitationLogic;
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Administrator)]
        public IActionResult CreateInvitation([FromBody] InvitationRequest invitationRequest)
        {
            var invitation = invitationRequest.ToEntity();

            var createdInvitation = _invitationLogic.CreateInvitation(invitation);

            var response = new InvitationResponse(createdInvitation);

            return CreatedAtAction(nameof(CreateInvitation), new { id = response.Id }, response);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateInvitationState([FromRoute] Guid id, [FromBody] InvitationRequest request)
        {
            var invitationLogic = _invitationLogic.UpdateInvitationState(id, request.State);
            InvitationResponse response = new InvitationResponse(invitationLogic);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Administrator)]
        public IActionResult DeleteInvitation([FromRoute]Guid id)
        {
            _invitationLogic.DeleteInvitation(id);
            return Ok();
        }
    }
}
