using cumin_api.Enums;
using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class ProjectService : DbService<Project> {
        public ProjectService(CuminApiContext context) : base(context) { }

        public Project Init(Project project, int uid) {
            // create project
            var project_ = Add(project);

            // assign creator as Project Manager
            var userProject = new UserProject { ProjectId = project_.Id, UserId = uid, UserRole =  UserRole.ProjectManager};
            context.UserProjects.Add(userProject);

            // create default empty roadmap
            Roadmap roadmap = new Roadmap{ ProjectId = project_.Id, Title = "Main"};
            SaveChanges();

            return project_;
        }

        public Project AddProjectAndLinkCreator(Project project, int userId) {
            // add the user
            var project_ = dbSet.Add(project);
            SaveChanges();
            // create relation with "UserProject" object between creator and the project
            var userProject = new UserProject { ProjectId = project_.Entity.Id, UserId = userId };
            context.UserProjects.Add(userProject);

            SaveChanges();

            return project_.Entity;
        }

        public IEnumerable<Project> GetAllProjectsForUser(int userId) {
            return context.UserProjects
                .Include(x => x.Project)
                .Where(x => x.UserId == userId)
                .Select(x => x.Project)
                .ToList();
        }

        public IEnumerable<User> GetAllUsersInProject(int projectId) {
            return context.UserProjects
                .Include(x => x.User)
                .Where(x => x.ProjectId == projectId)
                .Select(x => x.User)
                .ToList();
        }

        public bool CanUserAccessProject(int projectId, int userId) {
            return context.UserProjects.Any(x => x.ProjectId == projectId && x.UserId == userId);
        }

    }
}
