using AutoMapper;
using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    
    [Route("api/v1/project/{projectId}/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class SprintController: ControllerBase {
        private readonly Services.v2.SprintService sprintService;
        private IMapper mapper;
        public SprintController(Services.v2.SprintService sprintService, IMapper mapper) {
            this.sprintService = sprintService;
            this.mapper = mapper;
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpGet("{sprintId}")]
        public async Task<IActionResult> GetById(int sprintId, int projectId) {
            try {
                var sprint = await sprintService.GetWithIssues(sprintId, projectId);
                if (sprint == null) {
                    return new UnauthorizedObjectResult(new { message = $"Sprint not found in project. SprintId - {sprintId} and ProjectId - {projectId}" });
                }
                return Ok(sprint);
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetAll(int projectId) {
            try {
                var sprints = sprintService.GetAll(projectId).ToList();
                return Ok(sprints);
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpDelete("{sprintId}")]
        public async Task<IActionResult> DeleteSprint(int projectId, int sprintId) {
            try {
                var sprint = sprintService.Find(s => s.Id == sprintId && s.ProjectId == projectId);
                await sprintService.Delete(sprint);
                return Ok();
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateSprint([FromBody]SprintCreationDto dto, int projectId) {
            try {
                Sprint sprint = mapper.Map<Sprint>(dto);
                sprint.ProjectId = projectId;

                var sprint_ = await sprintService.AddAsync(sprint);
                return Ok(sprint_);
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpPatch("{sprintId}")]
        public async Task<IActionResult> PatchSprint([FromBody] JsonElement dto, int projectId, int sprintId) {
            try {
                var sprint = await sprintService.FindAsync(s => s.Id == sprintId && s.ProjectId == projectId);
                Helper.Mapper(dto, ref sprint);
                await sprintService.UpdateAsync(sprint);
                return Ok(sprint);
            } catch (Exception e) {
                throw e;
            }
        }
    }
}
