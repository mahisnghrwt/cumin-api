using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizationAttribute: Attribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationFilterContext context) {
            if (context.HttpContext.Items["userId"] == null)
                context.Result = new NotFoundResult();
        }
    }
}
