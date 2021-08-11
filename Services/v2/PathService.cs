using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class PathService:DbService<Models.Path> {
        public PathService(CuminApiContext context) : base(context) { }

        public IEnumerable<Models.Path> GetAllPathsInProject(int projectId) {
            return dbSet.Where(x => x.ProjectId == projectId);
        }
    }
}
