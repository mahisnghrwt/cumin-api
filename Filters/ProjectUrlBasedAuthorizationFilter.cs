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
            // context.HttpContext.Request.RouteValues["projectId"].ToString();
            // access the projectId from the url
            int projectId = GetProjectIdFromUrl(context.HttpContext.Request.Path.Value);
            // access userId from context.items
            int userId = Convert.ToInt32(context.HttpContext.Items["userId"]);
            // project service to check if the user can access this project
            if (projectService.CanUserAccessProject(projectId, userId) == false) {
                context.Result = new UnauthorizedResult();
            }
        }

        private int GetProjectIdFromUrl(string url) {
            int projectId = -1;
            if (String.IsNullOrEmpty(url) == false) {
                string target = "project/";
                int i = url.IndexOf(target);
                if (i != -1) {
                    try {
                        string s = url.Substring(i + target.Length);
                        if (String.IsNullOrEmpty(s) == false) {
                            int i2 = s.IndexOf('/');
                            int l = 0;
                            if (i2 != -1) {
                                l = i2;
                            }
                            else if (i2 == -1){
                                l = s.Length;
                            }
                            else {
                                return -1;
                            }

                            string pidInStr = s.Substring(0, l);
                            projectId = Convert.ToInt32(pidInStr);
                        }
                    } catch (ArgumentOutOfRangeException e) { } catch (FormatException e) { } catch (OverflowException e) { }
                }
            }

            return projectId;
        }
    }
}
