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
        private PathService pathService;
        private EpicService epicService;

        public RoadmapController(PathService pathService, EpicService epicService) {
            this.pathService = pathService;
            this.epicService = epicService;
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetRoadmap(int projectId) {
            RoadmapDto roadmapDto = new RoadmapDto { ProjectId = projectId };
            try {
                roadmapDto.Paths = pathService.GetAllPathsInProject(projectId);
                roadmapDto.Epics = epicService.GetAllInProject(projectId);
            } catch (Exception e) {
                throw e;
            }
            return Ok(roadmapDto);
        }
    }
}
