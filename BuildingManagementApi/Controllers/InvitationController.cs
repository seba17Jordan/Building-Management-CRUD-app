using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Domain;
using System;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BuildingManagementApi.Controllers
{
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationLogic _invitationLogic;

        public InvitationController(IInvitationLogic invitationLogic)
        {
            _invitationLogic = invitationLogic;
        }

        [HttpPost]
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
            throw new NotImplementedException();
        }
    }
}
