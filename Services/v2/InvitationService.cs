using cumin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class InvitationService : DbService2<ProjectInvitation> {
        public InvitationService(CuminApiContext context) : base(context) { }

        public async Task<ProjectInvitation> InviteAsync(string username, int inviterId, int projectId) {
            var target = context.Users.FirstOrDefault(x => x.Username == username);
            if (target == null)
                throw new SimpleException("User not found!");

            if (context.UserProjects.Any(x => x.UserId == target.Id && x.ProjectId == projectId))
                throw new SimpleException("User is already in team!");

            if (dbSet.Any(x => x.InviterId == inviterId && x.ProjectId == projectId && x.InviteeId == target.Id))
                throw new SimpleException("Duplicate invitation!");

            ProjectInvitation projectInvitation = new ProjectInvitation { InviteeId = target.Id, InviterId = inviterId, ProjectId = projectId };
            await dbSet.AddAsync(projectInvitation);

            await context.SaveChangesAsync();

            return projectInvitation;
        }
    }
}
