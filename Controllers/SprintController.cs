using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using cumin_api.Models.SocketMsgs;
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
    public class SprintController: ControllerBase {
        private readonly ISprintService sprintService;
        public SprintController(ISprintService sprintService) {
            this.sprintService = sprintService;
        }

        [HttpGet("project/{pid}")]
        public IActionResult GetSprintsByProjectId(int pid) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                var sprints = (List<Sprint>)sprintService.GetByProjectId(pid, uid);
                return Ok(sprints);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult Create(Sprint request) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            Sprint sprint;

            try {
                sprint = sprintService.Create(request, uid);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = sprint.ProjectId,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.SPRINT_CREATED,
                payload = sprint
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok(sprint);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult DeleteSprintById(int id) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            Sprint sprint;

            try {
                sprint = sprintService.DeleteById(id, uid);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = sprint.ProjectId,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.SPRINT_DELETED,
                payload = sprint
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok();
        }

    }
}
