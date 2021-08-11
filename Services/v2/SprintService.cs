using cumin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cumin_api.Services.v2 {
    public class SprintService: DbService<Sprint> {
        public SprintService(CuminApiContext context) : base(context) { }
        public IEnumerable<Sprint> GetAllSprintsByProject(int projectId) {
            return context
                .Sprints
                .Where(x => x.ProjectId == projectId);
        }

        public IEnumerable<Sprint> GetAllSprintsWithIssuesByProject(int projectId) {
            return context
                .Sprints
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.Issues);
        }

        public int GetProjectId(int sprintId) {
            return dbSet.FirstOrDefault(x => x.Id == sprintId).ProjectId;
        }

        public void DeleteInProject(int sprintId, int projectId) {
            Sprint sprint = dbSet.FirstOrDefault(x => x.Id == sprintId && x.ProjectId == projectId);
            dbSet.Remove(sprint);
            context.SaveChanges();
        }
    }
}
