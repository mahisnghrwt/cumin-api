using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class IssueService: DbService2<Issue> {
        public IssueService(CuminApiContext context) : base(context) { }


        public IEnumerable<Issue> GetAllIssuesByProject(int projectId) {
            return dbSet.Where(x => x.ProjectId == projectId).ToList();
        }

        public IEnumerable<Issue> GetAllIssuesByProjectDetailed(int projectId) {
            return dbSet.Include(i => i.Sprint).Include(i => i.Epic).Include(i => i.Reporter).Where(x => x.ProjectId == projectId).ToList();
        }

        public void DeleteInProject(int issueId, int projectId) {
            Issue issue = dbSet.FirstOrDefault(x => x.Id == issueId && x.ProjectId == projectId);
            dbSet.Remove(issue);
            context.SaveChanges();
        }

        public IEnumerable<Issue> GetAllWithSprint(int? sprintId, int projectId) {
            return dbSet.Where(i => i.SprintId == sprintId && i.ProjectId == projectId);
        }
    }
}
