using cumin_api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;

namespace cumin_api.Controllers {
    [CustomAuthorization]
    public class UserController: ControllerBase {
        private readonly Services.v2.UserService userService;
        private readonly Services.v2.ProjectService projectService;

        public UserController(Services.v2.UserService userService, Services.v2.ProjectService projectService) {
            this.userService = userService;
            this.projectService = projectService;
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
    }
}
