using cumin_api.Attributes;
using cumin_api.Models.DTOs;
using cumin_api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [CustomAuthorization]
    public class UserController: ControllerBase {
        private readonly Services.v2.UserService userService;
        private readonly Services.v2.ProjectService projectService;
        private readonly Services.StateManager stateManager;
        public UserController(Services.v2.UserService userService, Services.v2.ProjectService projectService, StateManager stateManager) {
            this.userService = userService;
            this.projectService = projectService;
            this.stateManager = stateManager;
        }

        [HttpGet]
        public IActionResult GetUser() {
            try {
                int userId = Convert.ToInt32(HttpContext.Items["userId"]);
                return Ok(userService.GetWithActiveProject(userId));
            } catch {
                return Unauthorized();
            }
        }

        [HttpPatch("active-project")]
        public IActionResult PatchActiveProject([FromBody] UserActiveSprintPatchDto dto) {
            try {
                int userId = Convert.ToInt32(HttpContext.Items["userId"]);            
                if (dto.ProjectId.HasValue == false || projectService.CanUserAccessProject(dto.ProjectId.Value, userId)) {
                    var user = userService.FindById(userId);

                    var oldProjectId = user.ActiveProjectId;

                    user.ActiveProjectId = dto.ProjectId;

                    var patchedUser = userService.Update(user);
                    // remove user from old project set
                    if (oldProjectId.HasValue != false) {
                        stateManager.RemoveUserFromProject(userId, oldProjectId.Value);
                    }

                    // add the user to new project set
                    if (dto.ProjectId.HasValue) {
                        stateManager.AddUserToProject(userId, dto.ProjectId.Value);
                    }


                    return Ok(patchedUser);
                } else {
                    return Unauthorized();
                }
            } catch {
                return Unauthorized();
            }
        }
    }
}
