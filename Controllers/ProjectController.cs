using AutoMapper;
using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace cumin_api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class ProjectController : ControllerBase {
        private readonly Services.v2.ProjectService projectService;
        private readonly Services.v2.UserService userService;
        private readonly IMapper mapper;

        public ProjectController(Services.v2.ProjectService projectService, Services.v2.UserService userService, IMapper mapper) {
            this.projectService = projectService;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreationDto projectRequest) {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            var project = new Project { Name = projectRequest.Name };

            try {
                var project_ = await projectService.AddProjectAndLinkCreator(project, userId);
                return Ok(project_);
            } catch (Exception e) {
                throw e;
            }
        }

        [HttpGet("{projectId}")]
        public IActionResult Get(int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                return Ok(projectService.FindById(projectId));
            } catch {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAll() {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });
            try {
                var userProjects = projectService.GetAllProjectsForUser(uid).ToList();
                List<ProjectDto> projects = new List<ProjectDto>();
                userProjects.ForEach(x => {
                    projects.Add(mapper.Map<ProjectDto>(x));
                });
                return Ok(projects);
            } catch {
                return NotFound();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpGet("{projectId}/member")]
        public IActionResult GetMembers(int projectId) {
            return Ok(projectService.GetAllUsersInProject(projectId));
        }

        [HttpPut("{projectId}/active-sprint")]
        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        public async Task<IActionResult> ChangeActiveSprint(int projectId, [FromBody] ActiveSprintChangeDto dto) {
            try {
                // get the project object
                var project = projectService.FindById(projectId);
                project.ActiveSprintId = dto.ActiveSprintId;
                // Update it
                 await projectService.UpdateAsync(project);
                return Ok(project);
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [HttpGet("{projectId}/active-sprint")]
        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        public async Task<IActionResult> GetActiveSprint(int projectId) {
            Sprint sprint = await projectService.GetActiveSprint(projectId);
            return Ok(sprint);
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpPatch("{projectId}/active-project")]
        public async Task<IActionResult> PatchProjectAsync(int projectId) {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            try {
                var user = await userService.SwitchActiveProjectAsync(userId, projectId);
                return Ok(user);
            } catch (SimpleException e) { 
                return new UnauthorizedObjectResult(new { message = e.Message });
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId) {
            try {
                var project = await projectService.FindAsync(x => x.Id == projectId);
                await projectService.DeleteAsync(project);
                return Ok();
            } catch (SimpleException e) {
                return new UnauthorizedObjectResult(new { message = e.Message });
            } catch (Exception e) {
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpDelete("{projectId}/user")]
        public async Task<IActionResult> LeaveProject (int projectId) {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            try {
                await projectService.LeaveProjectAsync(projectId, userId);
                return Ok();
            } catch (SimpleException e) {
                return new UnauthorizedObjectResult(new { message = e.Message });
            } catch (Exception e) {
                throw e;
            }
        }
    }
}
