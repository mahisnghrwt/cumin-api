using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace cumin_api.Filters {
    public class ProjectUrlBasedAuthorizationFilter : IAuthorizationFilter {
        private readonly Services.v2.ProjectService projectService;
        public ProjectUrlBasedAuthorizationFilter(Services.v2.ProjectService projectService) {
            this.projectService = projectService;
        }
        public void OnAuthorization(AuthorizationFilterContext context) {
            bool hasAccess = true;
            if (context.HttpContext.Request.RouteValues.TryGetValue("projectId", out Object projectId)) {
                int userId = Convert.ToInt32(context.HttpContext.Items["userId"]);
                // project service to check if the user can access this project
                if (projectService.CanUserAccessProject(Convert.ToInt32(projectId), userId) == false) {
                    hasAccess = false;
                }
            }
            else {
                hasAccess = false;
            }

            if (!hasAccess)
                context.Result = new UnauthorizedResult();
        }
    }
}
