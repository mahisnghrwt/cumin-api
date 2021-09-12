using cumin_api.Attributes;
using cumin_api.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [ApiController]
    [Route("api/v1/project/{projectId}/[controller]")]
    [CustomAuthorization]
    public class RoadmapController {

        [HttpGet("{roadmapId}")]
        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        public IAsyncResult GetById(int roadmapId, int projectId) {
            // get roadmap
            // get all paths
            // get all epics
        }
    }
}
