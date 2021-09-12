using cumin_api.Attributes;
using cumin_api.Filters;
using cumin_api.Models.DTOs;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cumin_api.Models;

namespace cumin_api.Controllers {
    [ApiController]
    [Route("api/v1/project/{projectId}/[controller]")]
    [CustomAuthorization]
    public class RoadmapController: ControllerBase {
        private RoadmapService roadmapService;

        public RoadmapController(RoadmapService roadmapService) {
            this.roadmapService = roadmapService;
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateRoadmap(int projectId, [FromBody] RoadmapCreationDto dto) {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            var roadmap = await roadmapService.Add(new Roadmap { ProjectId = projectId, Title = dto.Title, CreatorId = userId });
            return Ok(roadmap);
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpGet("{roadmapId}")]
        public IActionResult GetRoadmap(int roadmapId) {
            var roadmap = roadmapService.GetFull(roadmapId);
            return Ok(roadmap);
        }
    }
}
