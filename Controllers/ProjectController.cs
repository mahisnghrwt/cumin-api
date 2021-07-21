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
    public class ProjectController: ControllerBase {
        private readonly IProjectService projectService;
        public ProjectController(IProjectService projectService) {
            this.projectService = projectService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectRequest projectRequest) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(projectService.Create(projectRequest.Name, uid));
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{projectId}")]
        public IActionResult Get(int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if(uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                return Ok(projectService.GetById(projectId, uid));
            }
            catch {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAll() {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                return Ok(projectService.GetProjects(uid));
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
                return Ok(projectService.GetUsersForProject(projectId, userId));
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{pid}/active-sprint")]
        public IActionResult GetActiveSprint(int pid) {
            var userId = Helper.GetUid(HttpContext);
            if (userId == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(projectService.GetActiveSprintForProject(pid, userId));
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [HttpPut("{pid}/active-sprint/{sid}")]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult ActivateSprint(int pid, int sid) {
            var userId = Helper.GetUid(HttpContext);
            if (userId == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            List<Sprint> sprintStates;

            try {
                sprintStates = (List<Sprint>)projectService.ActivateSprintForProject(sid, pid, userId);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = pid,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.ACTIVE_SPRINT_UPDATED,
                payload = sprintStates
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok(sprintStates[1]);
        }

        [HttpDelete("{pid}/active-sprint")]
        public IActionResult UnactivateSprint(int pid) {
            var userId = Helper.GetUid(HttpContext);
            if (userId == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                projectService.UnactivateSprintForProject(pid, userId);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            return Ok();
        }
    }
}
