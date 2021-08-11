using cumin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services {
    public class StateManager {
        public Dictionary<int, UserState> Users { get; set; }
        public Dictionary<int, HashSet<int>> ProjectUsersMap { get; }

        public StateManager() {
            Users = new Dictionary<int, UserState>();
            ProjectUsersMap = new Dictionary<int, HashSet<int>>();
        }

        public void AddUserToProject(int uid, int pid) {
            if (ProjectUsersMap.ContainsKey(pid) == false) {
                ProjectUsersMap[pid] = new HashSet<int>();
            }

            ProjectUsersMap[pid].Add(uid);
        }

        public void RemoveUserFromProject(int uid, int pid) {
            if (ProjectUsersMap.ContainsKey(pid)) {
                ProjectUsersMap[pid].Remove(uid);
            }
        }


        /*
         * What are the websocket functionality I am looking for?
         * send data to single user
         * send data to users with a within a project
         * send data to user within a project with exceptions
         */
    }
}
