using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;
using cumin_api.Models;

namespace cumin_api.Services {
    public interface ISprintService {
        IEnumerable<Sprint> GetByProjectId(int pid, int uid);
        Sprint Create(Sprint sprint, int uid);
        Sprint DeleteById(int id, int uid);
    }
    public class SprintService : ISprintService {
        readonly private CuminApiContext dbContext;
        private const string UNAUTHORIZED_MSG = "User is unaouthorized to access this project!";

        private bool CanAccessProject(int pid, int uid) {
            return dbContext.UserProjects.Any(x => x.ProjectId == pid && x.UserId == uid);
        }

        public SprintService(CuminApiContext dbContext) {
            this.dbContext = dbContext;
        }

        public Sprint Create(Sprint sprint, int uid) {
            if (!CanAccessProject(sprint.ProjectId, uid)) {
                throw new SimpleException(UNAUTHORIZED_MSG);
            }

            var sprint_ = dbContext.Sprints.Add(sprint).Entity;
            dbContext.SaveChanges();

            return sprint_;
        }

        public Sprint DeleteById(int id, int uid) {
            var sprint = dbContext.Sprints.FirstOrDefault(x => x.Id == id);

            if (sprint == null || !CanAccessProject(sprint.ProjectId, uid)) {
                throw new SimpleException(UNAUTHORIZED_MSG);
            }

            var sprint_ = dbContext.Sprints.Remove(sprint).Entity;
            dbContext.SaveChanges();
            return sprint_;
        }

        public IEnumerable<Sprint> GetByProjectId(int pid, int uid) {
            if (!CanAccessProject(pid, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            var sprints = dbContext.Sprints
                .Include(x => x.Issues)
                .ThenInclude(x => x.Reporter)
                .Where(x => x.ProjectId == pid)
                .ToList();

            foreach (var sprint in sprints) {
                foreach (var issue in sprint.Issues) {
                    issue.Sprint = null;
                }
            }

            return sprints;
        }
    }
}
