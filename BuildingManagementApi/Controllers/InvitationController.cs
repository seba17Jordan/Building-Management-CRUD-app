using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.In;
using ModelsApi.Out;
using Domain;
using System;

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
    }
}
