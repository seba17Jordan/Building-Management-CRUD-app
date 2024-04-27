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
        public IActionResult CreateInvitation([FromBody] CreateInvitationRequest invitationRequest)
        {
            var invitation = new Invitation(invitationRequest.Email, invitationRequest.Name, invitationRequest.ExpirationDate);

            var createdInvitation = _invitationLogic.CreateInvitation(invitation);

            var response = new CreateInvitationResponse(createdInvitation);

            return CreatedAtAction(nameof(CreateInvitation), new { id = response.Id }, response);
        }

        [HttpPut("{id}/accept")]
        public IActionResult AcceptInvitation([FromRoute] Guid id, [FromBody] AcceptInvitationRequest request)
        {
            var response = new AcceptInvitationResponse(_invitationLogic.AcceptInvitation(id, request.Email, request.Password));
            return Ok(response);
        }

        [HttpPut("{id}/reject")]
        public IActionResult RejectInvitation([FromRoute] Guid id)
        {
            _invitationLogic.RejectInvitation(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[AuthenticationFilter([Roles.Administrator])]
        public IActionResult DeleteInvitation(Guid id)
        {
            _invitationLogic.DeleteInvitation(id);
            return NoContent();
        }
    }
}
