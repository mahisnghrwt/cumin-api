using cumin_api.Attributes;
using cumin_api.Enums;
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
    public class PathController:ControllerBase {
        private readonly PathService pathService;

        public PathController(PathService pathService) {
            this.pathService = pathService;
        }

        // get all paths in project
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetAllPathsInProject(int projectId) {
            try {
                var paths = pathService.GetAllPathsInProject(projectId);
                return Ok(paths);
            } catch {
                return Unauthorized();
            }
        }

        // create path
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpPost]
        public IActionResult CreatePath([FromBody] PathCreationDto dto, int projectId) {
            try {
                int uid = Convert.ToInt32(HttpContext.Items["userId"]);
                Models.Path path = new Models.Path { FromEpicId = dto.FromEpicId, ToEpicId = dto.ToEpicId, ProjectId = projectId };
                var path_ = pathService.Add(path);

                SocketMessageHeader sockHeader = new SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = projectId };
                SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.PATH_CREATED, payload = path_ };

                HttpContext.Items["SocketMessageHeader"] = sockHeader;
                HttpContext.Items["SocketMessage"] = sockMsg;

                return Ok(path_);
            } catch {
                return Unauthorized();
            }
        }

        // delete path
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        [HttpDelete("{pathId}")]
        public IActionResult DeletePathById(int pathId, int projectId) {
            try {
                var path = pathService.FindById(pathId);
                if (path.ProjectId != projectId) {
                    return Unauthorized();
                }
                pathService.DeleteById(pathId);

                SocketMessageHeader sockHeader = new SocketMessageHeader { broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST, targetId = projectId };
                SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.PATH_DELETED, payload = path};

                HttpContext.Items["SocketMessageHeader"] = sockHeader;
                HttpContext.Items["SocketMessage"] = sockMsg;

                return Ok();
            } catch {
                return Unauthorized();
            }
        }
    }
}
