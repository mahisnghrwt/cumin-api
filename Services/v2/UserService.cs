using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class UserService: DbService<User> {
        public UserService(CuminApiContext context): base(context) { }
        public User Authenticate(string username, string password) {
            return dbSet.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public User GetWithActiveProject(int userId) {
            return dbSet.Include(x => x.ActiveProject).FirstOrDefault(x => x.Id == userId);
        }
    }
}
