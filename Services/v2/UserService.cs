using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class UserService: DbService2<User> {
        public UserService(CuminApiContext context): base(context) { }
        public User Authenticate(string username, string password) {
            return dbSet.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public User GetWithActiveProject(int userId) {
            return dbSet.Include(x => x.ActiveProject).FirstOrDefault(x => x.Id == userId);
        }

        public string GetRoleInProject(int uid, int pid) {
            var up = context.UserProjects.FirstOrDefault(up => up.ProjectId == pid && up.UserId == uid);
            return up == null ? null : up.UserRole;
        }
    }
}
