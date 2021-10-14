using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models.DTOs;
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
        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetAllPathsInProject(int projectId) {
            try {
                var paths = pathService.GetAllPathsInProject(projectId);
                return Ok(paths);
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreatePath([FromBody] PathCreationDto dto, int projectId) {
            try {
                Models.Path path = new Models.Path { FromEpicId = dto.FromEpicId, ToEpicId = dto.ToEpicId, ProjectId = projectId };
                var path_ = await pathService.AddToProjectAsync(path, projectId);
                return Ok(path_);
            } catch (Exception e) {
                if (e is SimpleException) {
                    return NotFound(e.Message);
                }
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpDelete("{pathId}")]
        public async Task<IActionResult> DeletePathById(int pathId, int projectId) {
            try {
                await pathService.DeleteFromProjectAsync(pathId, projectId);
            } catch (Exception e) {
                if (e is SimpleException) {
                    return NotFound(e.Message);
                }
                throw e;
            }
            return Ok();
        }
    }
}
