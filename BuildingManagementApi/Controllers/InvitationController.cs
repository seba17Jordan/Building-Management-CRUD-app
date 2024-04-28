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
        public IActionResult RejectInvitationState([FromRoute] Guid id)
        {
            _invitationLogic.RejectInvitation(id);
            return Ok("Invitation Rejected");
        }

        /*[HttpPost("{id}")]
        public IActionResult AcceptInvitation([FromRoute] Guid id, [FromBody] ManagerRequest request)
        {
            //creo manager
            var manager = request.ToEntity();
            var createdManager = _invitationLogic.AcceptInvitation(id, manager);
            var response = new ManagerResponse(createdManager);
            return CreatedAtAction(nameof(AcceptInvitation), new { id = response.Id }, response);
        }*/

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
