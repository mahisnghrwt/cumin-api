using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace cumin_api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class ProjectController : ControllerBase {
        private readonly Services.v2.ProjectService projectService;
        public ProjectController(Services.v2.ProjectService projectService) {
            this.projectService = projectService;
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectCreationDto projectRequest) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            var project = new Project { Name = projectRequest.Name };

            try {
                return Ok(projectService.AddProjectAndLinkCreator(project, uid));
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{projectId}")]
        public IActionResult Get(int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                return Ok(projectService.FindById(projectId));
            } catch {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAll() {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                return Ok(projectService.GetAllProjectsForUser(uid));
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{projectId}/members")]
        public IActionResult GetMembers(int projectId) {
            var userId = Helper.GetUid(HttpContext);
            if (userId == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(projectService.GetAllUsersInProject(projectId));
            } catch {
                return NotFound();
            }
        }

        [HttpPut("{projectId}/active-sprint")]
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult ChangeActiveSprint(int pid, [FromBody] Models.DTOs.ActiveSprintChangeDto activeSprintChangeDto) {
            const string prev = "prev";
            const string current = "current";

            var userId = Helper.GetUid(HttpContext);
            if (userId == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            Dictionary<string, Project> projectStates = new Dictionary<string, Project>();
            Project projectPatched;

            try {
                // get the project object
                var project = projectService.FindById(pid);
                // store the current state as "prev"
                projectStates.Add(prev, project.DeepCopy());
                // change activeSprintId property
                project.ActiveSprintId = activeSprintChangeDto.ActiveSprintId;
                // Update it
                projectPatched = projectService.Update(project);
                // store the patched state as "current" in dict
                projectStates.Add(current, projectPatched);
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            // create socket message header and message, then attach it to the request context, so it will be accessible by filter responsible for forwarding the messages to websocket.
            Models.Socket.SocketMessageHeader sockHeader = new Models.Socket.SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = pid };
            Models.Socket.SocketMessage sockMessage = new Models.Socket.SocketMessage { eventName = (int)Enums.SOCKET_EVENT.ACTIVE_SPRINT_UPDATED, payload = projectStates };

            HttpContext.Items["SocketMessageHeader"] = sockHeader;
            HttpContext.Items["SocketMessage"] = sockMessage;

            return Ok(projectPatched);
        }
    }
}
