using cumin_api.Enums;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace cumin_api.Filters {
    public class RoleAuthorizationFilter : IAuthorizationFilter {
        private UserService userService;
        public RoleAuthorizationFilter(UserService userService) {
            this.userService = userService;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            int uid = Convert.ToInt32(context.HttpContext.Items["userId"]);
            int pid = Convert.ToInt32(context.HttpContext.Request.RouteValues["projectId"]);
            if (userService.GetRoleInProject(uid, pid) != UserRole.ProjectManager) {
                context.Result = new ForbidResult();
            }
        }
    }
}
