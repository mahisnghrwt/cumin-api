using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services {
    

    public class ProjectService: IProjectService {
        private readonly CuminApiContext dbContext;
        private const string UNAUTHORIZED_MSG = "User is unaouthorized to access this project!";

        public ProjectService(CuminApiContext dbContext) {
            this.dbContext = dbContext;
        }

        public Project Create(string name, int creatorId) {
            Project project = new Project { Name = name };
            var creator = dbContext.Users.FirstOrDefault(u => u.Id == creatorId);

            // by default add the creator as the user
            var userProject = new UserProject { Project = project, User = creator };

            dbContext.Projects.Add(project);
            dbContext.UserProjects.Add(userProject);
            // if the number of changes is less than or equal to 0, that means something went wrong
            if (dbContext.SaveChanges() <= 0)
                return null;
            
            return project;
        }

        public int AddUser(int projectId, int userId) {
            var user = dbContext.Users.FirstOrDefault(user => user.Id == userId);
            var project = dbContext.Projects.FirstOrDefault(project => project.Id == projectId);

            if (user == null || project == null)
                return -1;

            var userProject = new UserProject { User = user, Project = project };
                        

            dbContext.UserProjects.Add(userProject);
            dbContext.SaveChanges();

            return projectId;
        }

        public Project Get(int projectId) {
            return dbContext.Projects.Include(p => p.UserProjects).ThenInclude(up => up.User).FirstOrDefault(project => project.Id == projectId);
        }

        public Project GetById(int projectId, int userId) {
            return dbContext.UserProjects
                .Include(x => x.Project)
                .Where(x => x.ProjectId == projectId && x.UserId == userId)
                .Select(x => x.Project)
                .FirstOrDefault();
        }

        public IEnumerable<Project> GetProjects(int userId) {
            return dbContext.UserProjects.Include(up => up.Project).Where(up => up.UserId == userId).Select(up => up.Project).ToList();
        }

        public IEnumerable<User> GetUsers(int projectId) {
            return dbContext.UserProjects.Where(x => x.ProjectId == projectId).Include(x => x.User).Select(x => x.User).ToList();
        }

        public IEnumerable<User> GetUsersForProject(int projectId, int userId) {
            return dbContext.UserProjects
                .Where(x => x.ProjectId == projectId && x.UserId == userId)
                .Include(x => x.User)
                .Select(x => x.User);
        }

        public ProjectInvitation InviteByUsername(int projectId, int inviterId, string inviteeUsername) {
            // get invitee id
            int inviteeId = dbContext.Users.FirstOrDefault(x => x.Username == inviteeUsername).Id;
            // check whether the inviter belongs to this project
            var belongs = dbContext.UserProjects.Any(x => x.UserId == inviterId && x.ProjectId == projectId);
            if (!belongs) {
                throw new SimpleException(UNAUTHORIZED_MSG);
            }

            var invitation = dbContext.ProjectInvitations.Add(new ProjectInvitation { InviteeId = inviteeId, InviterId = inviterId, ProjectId = projectId });
            dbContext.SaveChanges();

            return invitation.Entity;
        }

        public ProjectInvitation AcceptInvitation(int invitationId, int userId) {
            // check if this invitation is for this user
            var invite = dbContext.ProjectInvitations
                .Include(x => x.Project)
                .Include(x => x.Invitee)
                .FirstOrDefault(x => x.Id == invitationId);

            dbContext.UserProjects.Add(new UserProject { ProjectId = invite.ProjectId, UserId = invite.InviteeId });
            dbContext.ProjectInvitations.Remove(invite);
            dbContext.SaveChanges();
            
            return invite;
        }
        public ProjectInvitation RejectInvitation(int invitationId, int userId) {
            // check if this invitation is for this user
            var invite = dbContext.ProjectInvitations
                .Include(x => x.Invitee)
                .FirstOrDefault(x => x.Id == invitationId);

            dbContext.ProjectInvitations.Remove(invite);
            dbContext.SaveChanges();

            return invite;
        }

        public IEnumerable<ProjectInvitation> GetPendingInvitations(int userId) {
            return dbContext.ProjectInvitations.Include(x => x.Inviter).Include(x => x.Project).Where(x => x.InviteeId == userId).ToList();
        }

        public bool BelongsTo(int projectId, int userId) {
            return dbContext.UserProjects.Where(up => up.ProjectId == projectId && up.UserId == userId).Any();
        }

        public Sprint GetActiveSprintForProject(int pid, int uid) {
            if (!BelongsTo(pid, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            //var activeSprint = dbContext.ActiveSprintProject
            //    .Include(x => x.Sprint)
            //    .ThenInclude(x => x.Issues)
            //    .FirstOrDefault(x => x.ProjectId == pid)
            //    .Sprint;

            //foreach(var issue in activeSprint.Issues) {
            //    issue.Sprint = null;
            //}

            //return activeSprint;
            return null;
        }

        public IEnumerable<Sprint> ActivateSprintForProject(int sid, int pid, int uid) {
            if (!BelongsTo(pid, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            // remove the already active sprint from this project first
            //Sprint prev = new Sprint();
            ////var prev = dbContext.ActiveSprintProject
            ////    .Include(x => x.Sprint)
            ////    .FirstOrDefault(x => x.ProjectId == pid);

            //Sprint old = null;

            //if (prev != null) {
            //    old = new Sprint(prev.Sprint);
            //    prev.SprintId = sid;
            //}
            //else {
            //    //prev = dbContext.ActiveSprintProject.Add(new ActiveSprintProject { SprintId = sid, ProjectId = pid }).Entity;
            //}
            //dbContext.SaveChanges();

            //dbContext.Entry(prev)
            //    .Reference(x => x.Sprint)
            //    .Load();

            //// just to remove the cycle in resulting json
            //prev.Sprint.ActiveSprint = null;

            //return new List<Sprint>() { old, prev.Sprint};
            return null;
        }

        public void UnactivateSprintForProject(int pid, int uid) {
            if (!BelongsTo(pid, uid))
                throw new SimpleException(UNAUTHORIZED_MSG);

            //var sprint = dbContext.ActiveSprintProject.FirstOrDefault(x => x.ProjectId == pid);
            //dbContext.ActiveSprintProject.Remove(sprint);
        }
    }
}