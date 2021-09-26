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
using AutoMapper;

namespace cumin_api.Controllers {
    [ApiController]
    [Route("api/v1/project/{projectId}/[controller]")]
    [CustomAuthorization]
    public class RoadmapController: ControllerBase {
        private RoadmapService roadmapService;
        private IMapper mapper;

        public RoadmapController(RoadmapService roadmapService, IMapper mapper) {
            this.roadmapService = roadmapService;
            this.mapper = mapper;
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
        [HttpGet]
        public IActionResult GetAllRoadmapInProject(int projectId) {
            IEnumerable <Roadmap> roadmaps;
            if (HttpContext.Request.Query.ContainsKey("detailed") && HttpContext.Request.Query["detailed"] == "true") {
                roadmaps = roadmapService.GetAllInDetailFromProject(projectId);
                var roadmaps_ = roadmaps.Select(r => {
                    return new {
                        id = r.Id,
                        projectId = r.ProjectId,
                        epics = r.RoadmapEpics.Select(re => {
                            return mapper.Map<EpicDto>(re);
                        }),
                        paths = r.RoadmapPaths.Select(rp => {
                            return new {
                                projectId = rp.Path.ProjectId,
                                roadmapId = rp.RoadmapId,
                                id = rp.PathId,
                                fromEpicId = rp.Path.FromEpicId,
                                toEpicId = rp.Path.ToEpicId,
                            };
                        })
                    };
                });
                return Ok(roadmaps_);
            }
            else {
                roadmaps = roadmapService.GetAllFromProject(projectId);
            }
            return Ok(roadmaps);
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
