using cumin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services {
    public interface IProjectService {
        // create project
        Project Create(string name, int creatorId);
        // get project by id
        Project GetById(int projectId, int userId);
        // get all projects for user
        IEnumerable<Project> GetProjects(int userId);
        // get all the members of a project
        IEnumerable<User> GetUsersForProject(int projectId, int userId);
        bool BelongsTo(int projectId, int userId);
        ProjectInvitation InviteByUsername(int projectId, int inviterId, string inviteeUsername);

        ProjectInvitation AcceptInvitation(int invitationId, int userId);
        ProjectInvitation RejectInvitation(int invitationId, int userId);
        IEnumerable<ProjectInvitation> GetPendingInvitations(int userId);

        // active-sprint functions
        // get active sprint of the project
        Sprint GetActiveSprintForProject(int pid, int uid);
        // make sprint as active
        IEnumerable<Sprint> ActivateSprintForProject(int sid, int pid, int uid);
        // remove active sprint if any
        void UnactivateSprintForProject(int pid, int uid);
    }
}
