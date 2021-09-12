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
    [Route("api/v1/project/{projectId}/roadmap/{roadmapId}/epic")]
    [CustomAuthorization]
    public class EpicController2: ControllerBase {

        private readonly EpicService epicService;
        public EpicController2(EpicService epicService) {
            this.epicService = epicService;
        }



        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateEpic([FromBody] EpicCreationDto dto, int projectId, int roadmapId) {
            try {
                int uid = Convert.ToInt32(HttpContext.Items["userId"]);
                Epic epic = new Epic { StartDate = dto.StartDate, EndDate = dto.EndDate, Row = dto.Row, Title = dto.Title, ProjectId = projectId };
                var epic_ = await epicService.AddToRoadmapAsync(epic, roadmapId);

                return Ok(epic_);
            } catch {
                return Unauthorized();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpPatch("{epicId}")]
        public IActionResult PatchEpicDuration(int epicId, int roadmapId, int projectId, [FromBody] EpicDurationPatchDto dto) {
            try {
                
                List<Epic> epicStates = new List<Epic>();

                // if the epic is in the roadmap
                var epic = epicService.FindById(epicId);
                if (!epic.RoadmapEpics.Any(re => re.RoadmapId == roadmapId))
                    return Unauthorized();

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
                SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.EPIC_UPDATED, payload = epicStates };

                HttpContext.Items["SocketMessageHeader"] = sockHeader;
                HttpContext.Items["SocketMessage"] = sockMsg;

                return Ok(patched);
            } catch (Exception e) {
                return Unauthorized();
            }
        }

    }
}
