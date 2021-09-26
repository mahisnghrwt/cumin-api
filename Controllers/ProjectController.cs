using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class ProjectController : ControllerBase {
        private readonly Services.v2.ProjectService projectService;
        public ProjectController(Services.v2.ProjectService projectService) {
            this.projectService = projectService;
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
                return Ok(projectService.GetAllProjectsForUser(uid));
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{projectId}/members")]
        public IActionResult GetMembers(int projectId) {
            var userId = Helper.GetUid(HttpContext);
            if (userId == -1)
                return BadRequest(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(projectService.GetAllUsersInProject(projectId));
            } catch {
                return NotFound();
            }
        }

        [HttpPut("{projectId}/active-sprint")]
        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        public async Task<IActionResult> ChangeActiveSprint(int pid, [FromBody] Models.DTOs.ActiveSprintChangeDto activeSprintChangeDto) {

            int userId = Convert.ToInt32(HttpContext.Items["userId"]);

            try {
                // get the project object
                var project = projectService.FindById(pid);
                project.ActiveSprintId = activeSprintChangeDto.ActiveSprintId;
                // Update it
                 await projectService.UpdateAsync(project);
                return Ok(project);
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }
    }
}
