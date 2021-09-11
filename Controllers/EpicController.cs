using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Models.Socket;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    
    [ApiController]
    [Route("api/v1/project/{projectId}/[controller]")]
    [CustomAuthorization]
    public class EpicController: ControllerBase {

        private readonly EpicService epicService;
        public EpicController(EpicService epicService) {
            this.epicService = epicService;
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpPost]
        public IActionResult CreateEpic([FromBody] EpicCreationDto dto, int projectId) {
            try {
                int uid = Convert.ToInt32(HttpContext.Items["userId"]);
                Epic epic = new Epic { StartDate = dto.StartDate, EndDate = dto.EndDate, Row = dto.Row, Title = dto.Title, ProjectId = projectId };
                var epic_ = epicService.Add(epic);

                SocketMessageHeader sockHeader = new SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = projectId };
                SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.EPIC_CREATED, payload = epic_ };

                HttpContext.Items["SocketMessageHeader"] = sockHeader;
                HttpContext.Items["SocketMessage"] = sockMsg;

                return Ok(epic_);
            } catch {
                return Unauthorized();
            }
        }

        // get all epics in project
        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetAllEpicsInProject(int projectId) {
            try {
                var epics = epicService.GetAllInProject(projectId).ToDictionary(x => x.Id.ToString());
                return Ok(epics);
            } catch {
                return Unauthorized();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpPatch("{epicId}")]
        public IActionResult PatchEpicDuration(int epicId, int projectId, [FromBody] EpicDurationPatchDto dto) {
            try {
                List<Epic> epicStates = new List<Epic>();

                var epic = epicService.FindById(epicId);
                if (epic == null || epic.ProjectId != projectId) {
                    // return Unauthorized();
                    return Unauthorized();
                }

                // save copy of original 
                epicStates.Add(new Epic(epic));

                // patch
                epic.StartDate = dto.StartDate;
                epic.EndDate = dto.EndDate;

                // update
                var patched = epicService.Update(epic);
                // store patched version in list
                epicStates.Add(patched);

                SocketMessageHeader sockHeader = new SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = projectId };
                SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.EPIC_UPDATED, payload = epicStates};

                HttpContext.Items["SocketMessageHeader"] = sockHeader;
                HttpContext.Items["SocketMessage"] = sockMsg;

                return Ok(patched);
            } catch (Exception e){
                return Unauthorized();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpDelete("{epicId}")]
        public IActionResult DeleteEpicById(int epicId, int projectId) {
            try {
                var epic = epicService.FindById(epicId);
                if (epic.ProjectId != projectId) {
                    return Unauthorized();
                }
                epicService.DeleteById(epicId);

                SocketMessageHeader sockHeader = new SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = projectId };
                SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.EPIC_DELETED, payload = epic };

                HttpContext.Items["SocketMessageHeader"] = sockHeader;
                HttpContext.Items["SocketMessage"] = sockMsg;

                return Ok();
            } catch {
                return Unauthorized();
            }
        }
    }
}
