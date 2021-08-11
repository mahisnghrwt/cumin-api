using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace cumin_api.Controllers {
    
    [Route("api/v1/project/{projectId}/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class SprintController: ControllerBase {
        private readonly Services.v2.SprintService sprintService;
        public SprintController(Services.v2.SprintService sprintService) {
            this.sprintService = sprintService;
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetAllByProjectAsDict(int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(sprintService.GetAllSprintsWithIssuesByProject(projectId).ToDictionary(x => x.Id.ToString()));
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }


        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpPost]
        public IActionResult CreateSprint([FromBody] SprintCreationDto dto, int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            if (projectId != dto.ProjectId) {
                return BadRequest();
            }

            Sprint sprint = new Sprint { Title = dto.Title, ProjectId = dto.ProjectId};
            Sprint sprintCreated;

            try {
                sprintCreated = sprintService.Add(sprint);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            Models.Socket.SocketMessageHeader sockHeader = new Models.Socket.SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = dto.ProjectId };
            Models.Socket.SocketMessage sockMessage = new Models.Socket.SocketMessage { eventName = (int)Enums.SOCKET_EVENT.SPRINT_CREATED, payload = sprintCreated };

            HttpContext.Items["SocketMessageHeader"] = sockHeader;
            HttpContext.Items["SocketMessage"] = sockMessage;

            return Ok(sprintCreated);
        }

        [HttpDelete("{sprintId}")]
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult DeleteSprintById(int projectId, int sprintId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                sprintService.DeleteInProject(sprintId, projectId);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            Models.Socket.SocketMessageHeader sockHeader = new Models.Socket.SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = projectId };
            Models.Socket.SocketMessage sockMessage = new Models.Socket.SocketMessage { eventName = (int)Enums.SOCKET_EVENT.SPRINT_DELETED, payload = sprintId };

            HttpContext.Items["SocketMessageHeader"] = sockHeader;
            HttpContext.Items["SocketMessage"] = sockMessage;

            return Ok();
        }
    }
}
