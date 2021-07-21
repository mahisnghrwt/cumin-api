using cumin_api.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Middlewares {
    public class ProjectAuthorizationMiddleware {
        RequestDelegate next;
        public ProjectAuthorizationMiddleware(RequestDelegate next) {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IProjectService projectService) {
            // check if the user in the request is part of the project
            // /api/v1/project/{projectId}
            //var path = context.Request.Path.Value;
            //var pathDir = path.Split('/');

            int projectId = -1;
            int userId = -1;
            bool hasProjectAndUserId = true;

            if (context.Request.Query.ContainsKey("project")) {
                try {
                    projectId = Convert.ToInt32(context.Request.Query["project"]);
                    userId = Convert.ToInt32(context.Items["userId"]);
                } catch {
                    hasProjectAndUserId = false;
                }
            }

            if (hasProjectAndUserId && projectService.BelongsTo(projectId, userId)) {
                context.Items["CanAccessProject"] = true;
                context.Items["projectId"] = projectId;
            }


            //if (pathDir.Length >= 5) {
            //    // this is string
            //    projectId = pathDir[4];

            //    Console.WriteLine($"ProjectId {projectId}");

            //    bool parseSuccesful = int.TryParse(projectId, out projectId_);
            //    if (parseSuccesful == true) {
            //        context.Items["projectId"] = projectId_;

            //        // parse userId from object to int
            //        try {
            //            // we must have the userId store inside the httpContext.items by the JwtMiddleware
            //            var userId = Convert.ToInt32(context.Items["userId"]);

            //            Console.WriteLine($"UserId {userId}");

            //            if (projectService.BelongsTo(projectId_, userId)) {
            //                context.Items["CanAccessProject"] = true;
            //            }
            //        } catch { }
            //    }
            //}

            await next(context);
        }
    }
}
