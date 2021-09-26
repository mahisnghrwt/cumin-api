using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
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
    [Route("api/v1/project/{projectId}/roadmap/{roadmapId}/[controller]")]
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
                var paths = pathService.GetAllPathsInProject(projectId).ToDictionary(x =>x.Id.ToString());
                return Ok(paths);
            } catch {
                return Unauthorized();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreatePath([FromBody] PathCreationDto dto, int roadmapId, int projectId) {
            try {
                int uid = Convert.ToInt32(HttpContext.Items["userId"]);
                Models.Path path = new Models.Path { FromEpicId = dto.FromEpicId, ToEpicId = dto.ToEpicId, ProjectId = projectId };
                var path_ = await pathService.AddToRoadmapAsync(path, roadmapId);

                return Ok(path_);
            } catch (Exception e) {
                throw e;
                return Unauthorized();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpDelete("{pathId}")]
        public async Task<IActionResult> DeletePathById(int pathId, int roadmapId) {
            try {
                await pathService.DeleteFromRoadmapAsync(pathId, roadmapId);

            } catch {
                return Unauthorized();
            }

            return Ok();
        }
    }
}
