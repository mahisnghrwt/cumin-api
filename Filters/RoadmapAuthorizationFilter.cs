using cumin_api.Enums;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Filters {
    public class RoadmapAuthorizationFilter : IAuthorizationFilter {
        private RoadmapService roadmapService;
        public RoadmapAuthorizationFilter(RoadmapService roadmapService) {
            this.roadmapService = roadmapService;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            int uid = Convert.ToInt32(context.HttpContext.Items["userId"]);
            int rid = Convert.ToInt32(context.HttpContext.Request.RouteValues["roadmapId"]);
            var roadmap = roadmapService.FindById(rid);
            if (roadmap == null || roadmap.CreatorId != uid) {
                ObjectResult result = new ObjectResult(new { message = $"Cannot access roadmap: {roadmap.Id}." });
                result.StatusCode = 401;
                context.Result = result;
            }
        }
    }
}
