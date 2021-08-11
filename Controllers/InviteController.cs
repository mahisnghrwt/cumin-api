using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using cumin_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {

    [Route("api/v1/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class InviteController: ControllerBase {
        private const string SOCK_MSG = "sockMsg";
        private readonly IProjectService projectService;
        public InviteController(IProjectService projectService) {
            this.projectService = projectService;
        }

        // send invitation
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpPost]
        public IActionResult Invite([FromBody] ProjectInvitationRequest invitation) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            ProjectInvitation invite;

            try {
                invite = projectService.InviteByUsername(invitation.ProjectId, uid, invitation.InviteeUsername);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            // forward the invite to the invitee using socket
            //var socketMsgs = new List<Object>();
            //socketMsgs.Add(new GeneralSockMsg { 
            //    id = invite.InviteeId, 
            //    type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.TARGET, 
            //    eventName = (int)SOCKET_EVENT.INVITATION_RECEIVED, 
            //    payload = invite 
            //});
            //HttpContext.Items[SOCK_MSG] = socketMsgs;

            return Ok(invite);
        }

        // accept invitation
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpGet("accept/{inviteId}")]
        public IActionResult Accept(int inviteId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            // accept invite
            // get user details and project details
            ProjectInvitation invite;
            try {
                invite = projectService.AcceptInvitation(inviteId, uid);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }


            //var socketMsgs = new List<Object>();
            //socketMsgs.Add(new RPCSockMsg { 
            //    payload = new { 
            //        function = (int)CommonEnums.RPC_FUNCTION.CONNNECTION_SERVICE_SUBSCRIBE_PROJECTS, 
            //        parameters = new { 
            //            uid = invite.InviteeId, 
            //            projects = new int[] { 
            //                invite.ProjectId 
            //            } 
            //        } 
            //    } 
            //});
            //// return info about the user to other members of project
            //// socketMsgs.Add(new { id = invite.ProjectId, type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST, eventName = SOCKET_EVENT.NEW_USER_JOINED, payload = invite });
            //socketMsgs.Add(new GeneralSockMsg {
            //    id = invite.ProjectId,
            //    type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
            //    eventName = (int)SOCKET_EVENT.NEW_USER_JOINED,
            //    payload = invite
            //});
            //HttpContext.Items[SOCK_MSG] = socketMsgs;

            // return project info to the accepting user
            return Ok(invite.Project);
        }

        // reject invitation
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpGet("reject/{inviteId}")]
        public IActionResult Reject(int inviteId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            ProjectInvitation invite;
            try {
                invite = projectService.RejectInvitation(inviteId, uid);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            //// return info about the user to other members of project
            //var socketMsgs = new List<Object>();
            //socketMsgs.Add(new GeneralSockMsg { 
            //    id = invite.InviterId, 
            //    type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.TARGET, 
            //    eventName = (int)SOCKET_EVENT.INVITATION_REJECTED, 
            //    payload = invite 
            //});
            //HttpContext.Items[SOCK_MSG] = socketMsgs;

            return Ok(invite);
        }

        // get all pending invitations
        // reject invitation
        [HttpGet("pending")]
        public IActionResult GetPending() {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(projectService.GetPendingInvitations(uid));
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }
    }
}
