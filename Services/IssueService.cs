using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;
using cumin_api.Models;

namespace cumin_api.Services {
    public interface IIssueService {
        Issue Create(Issue issue);
        Issue Get(int id, int uid);
        IEnumerable<Issue> GetAll();
        IEnumerable<Issue> GetByProjectId(int pid, int uid);
        IEnumerable<Issue> GetBySprintId(int? sid, int pid, int uid);
        Issue Delete(int id, int uid);
        IEnumerable<Issue> Update(Issue issue, int uid);
        Issue UpdateOnly(Issue issue, int uid);
    }
    public class IssueService : IIssueService {
        readonly private CuminApiContext dbContext;
        private const string UNAUTHORIZED_MSG = "User is unaouthorized to access this project!";

        private bool CanAccessProject(int pid, int uid) {
            return dbContext.UserProjects.Any(x => x.ProjectId == pid && x.UserId == uid);
        }

        public IssueService(CuminApiContext dbContext) {
            this.dbContext = dbContext;
        }

        public Issue Create(Issue issue_) {
            // check if the user can access the project
            if (!CanAccessProject(issue_.ProjectId, issue_.ReporterId)) {
                // throw error
                // return
                throw new SimpleException(UNAUTHORIZED_MSG);
            }

            // Issue issue = new Issue(iissue);
            var issue = dbContext.Issues.Add(issue_);
            issue.Reference(x => x.Reporter).Load();
            dbContext.SaveChanges();

            return issue.Entity;
        }

        public Issue Delete(int id, int uid) {
            var issue = dbContext.Issues.FirstOrDefault(x => x.Id == id);

            if (issue == null || !CanAccessProject(issue.ProjectId, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            var issue_ = dbContext.Issues.Remove(issue).Entity;
            dbContext.SaveChanges();
            return issue_;
        }

        public Issue Get(int id, int uid) {

            var issue = dbContext.Issues
                .Include(x => x.Project)
                .Include(x => x.Reporter)
                .Include(x => x.AssignedTo)
                .FirstOrDefault(x => x.Id == id);

            if (!CanAccessProject(issue.ProjectId, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            return issue;
        }

        public IEnumerable<Issue> GetAll() {
            return dbContext.Issues
                .Include(x => x.Project)
                .Include(x => x.Reporter)
                .Include(x => x.AssignedTo)
                .ToList();
        }

        public IEnumerable<Issue> GetByProjectId(int pid, int uid) {
            if (!CanAccessProject(pid, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            return dbContext.Issues
                .Include(x => x.Project)
                .Include(x => x.Reporter)
                .Include(x => x.AssignedTo)
                .Where(x => x.ProjectId == pid)
                .ToList();
        }

        public IEnumerable<Issue> GetBySprintId(int? sid, int pid, int uid) {
            // var sprint = dbContext.Sprints.FirstOrDefault(x => x.Id == sid);
            if (!CanAccessProject(pid, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            return dbContext.Issues
                .Include(x => x.Project)
                .Include(x => x.Reporter)
                .Include(x => x.AssignedTo)
                .Where(x => x.SprintId == sid && x.ProjectId == pid)
                .ToList();
        }

        public IEnumerable<Issue> Update(Issue issue, int uid) {
            var currentIssue = dbContext.Issues.FirstOrDefault(x => x.Id == issue.Id);
            if (currentIssue == null || !CanAccessProject(currentIssue.ProjectId, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);
            
            var prevIssue = new Issue(currentIssue);

            dbContext.Entry(currentIssue).Reference(x => x.Reporter).Load();

            currentIssue.CopyForUpdate(issue);
            dbContext.SaveChanges();

            return new List<Issue> { prevIssue, currentIssue};
        }

        /// <summary>
        /// Id, ProjectId, ReporterId, cannot be changed of an Issue
        /// </summary>
        /// <param name="issue"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Issue UpdateOnly(Issue issue, int uid) {
            var currentIssue = dbContext.Issues.FirstOrDefault(x => x.Id == issue.Id);
            if (currentIssue == null || !CanAccessProject(currentIssue.ProjectId, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            dbContext.SaveChanges();

            return currentIssue;
        }
    }
}
