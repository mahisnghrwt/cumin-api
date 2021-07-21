using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Attributes {

    public class ProjectAuthorizationAttribute : Attribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationFilterContext context) {
            if (context.HttpContext.Items["CanAccessProject"] == null)
                context.Result = new NotFoundObjectResult(new { message = $"User: {context.HttpContext.Items["userId"]} is not authorized to access project: {context.HttpContext.Items["projectId"]}"});
        }
    }
}
