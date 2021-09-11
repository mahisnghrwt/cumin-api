using cumin_api.Enums;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Filters {
    public class RoleAuthorizationFilter : IAuthorizationFilter {
        private UserService userService;
        public RoleAuthorizationFilter(UserService userService) {
            this.userService = userService;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            int uid = (int)context.HttpContext.Items["userId"];
            var user = userService.FindById(uid);
            if (user.Role != UserRole.ProjectManager)
                context.Result = new UnauthorizedResult();
        }
    }
}
