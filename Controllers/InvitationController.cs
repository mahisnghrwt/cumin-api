using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {

    [Route("api/v1/project/{projectId}/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class InvitationController: ControllerBase {
        private readonly InvitationService invitationService;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly HubUserService hubUserService;

        private const string INVITE_NOTIFICATION = "ReceiveMessage";

        public InvitationController(InvitationService invitationService, IHubContext<NotificationHub> notificationHub, HubUserService hubUserService) {
            this.invitationService = invitationService;
            this.notificationHub = notificationHub;
            this.hubUserService = hubUserService;
        }

        // send invitation
        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> Invite([FromBody] InviteRequestDto dto, int projectId) {
            var userId = Convert.ToInt32(HttpContext.Items["userId"]);

            try {
                var invite = await invitationService.InviteAsync(dto.Username, userId, projectId);

                // forward the notification to 
                if (hubUserService.Users.ContainsKey(invite.InviteeId)) {
                    string targetConnectionStr = hubUserService.Users[invite.InviteeId];
                    await notificationHub.Clients.Client(targetConnectionStr).SendAsync(INVITE_NOTIFICATION, invite);
                }

                return Ok(invite);
            } catch (Exception e) {
                if (e is SimpleException)
                    return Unauthorized(new { message = e.Message });
                throw e;
            }
        }
    }
}
