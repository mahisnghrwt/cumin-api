using cumin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cumin_api.Services.v2 {
    public class SprintService : DbService2<Sprint> {
        public SprintService(CuminApiContext context) : base(context) { }

        public async Task<Sprint> GetWithIssues(int sprintId, int projectId) {
            return await dbSet.Include(s => s.Issues).FirstOrDefaultAsync(s => s.Id == sprintId && s.ProjectId == projectId);
        }

        public IEnumerable<Sprint> GetAll(int projectId) {
            return dbSet.Include(s => s.Issues).Where(s => s.ProjectId == projectId);
        }
    }
}
