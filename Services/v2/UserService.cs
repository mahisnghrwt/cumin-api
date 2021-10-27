using cumin_api.Enums;
using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class UserService: DbService2<User> {
        public UserService(CuminApiContext context): base(context) { }
        public User Authenticate(string username, string password) {
            return dbSet.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public UserProject GetWithActiveProject(int userId) {
            var user = dbSet.FirstOrDefault(x => x.Id == userId);
            if (user.ActiveProjectId == null) {
                return new UserProject { User = user };
            }
            return context.UserProjects.Include(x => x.Project).Include(x => x.User).FirstOrDefault(x => x.UserId == userId && x.ProjectId == user.ActiveProjectId);
            // return dbSet.Include(x => x.ActiveProject).FirstOrDefault(x => x.Id == userId);
        }

        public string GetRoleInProject(int uid, int pid) {
            var up = context.UserProjects.FirstOrDefault(up => up.ProjectId == pid && up.UserId == uid);
            return up == null ? null : up.UserRole;
        }

        public async Task<User> SwitchActiveProjectAsync(int userId, int projectId) {
            var canAccessProject = await context.UserProjects.AnyAsync(x => x.UserId == userId && x.ProjectId == projectId);
            if (canAccessProject) { 
                var user = await FindAsync(x => x.Id == userId);
                user.ActiveProjectId = projectId;
                await UpdateAsync(user);
                return user;
            }

            throw new SimpleException($"User is not in project with id: {projectId}.");
        }
    }
}
