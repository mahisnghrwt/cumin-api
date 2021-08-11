using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services {
    public class SockService {
        private readonly StateManager stateManager;
        public SockService(StateManager stateManager) {
            this.stateManager = stateManager;
        }

        public async Task Broadcast(int projectId, Object data, HashSet<int> exceptions) {
            try {
                // return Task if the project is not listed in "projectUsersMap"
                if (stateManager.ProjectUsersMap.ContainsKey(projectId) == false) {
                    return;
                }

                // get the list of users in this project from stateManager
                var users = stateManager.ProjectUsersMap[projectId].ToList();
                foreach(var uid in users) {
                    if (exceptions.Contains(uid)) {
                        continue;
                    }
                    if (stateManager.Users.ContainsKey(uid) && stateManager.Users[uid].isActive()) {
                        await stateManager.Users[uid].sendData(data);
                    }
                }
                // get all the sockets for the corresponding active users
                // if the socket is open send the data ony by one
                // later implement it as async
            } catch(Exception e) {
                throw e;
            }
        }

        public async Task Target(int uid, Object data) {
            try {
                if (stateManager.Users.ContainsKey(uid) && stateManager.Users[uid].isActive()) {
                    await stateManager.Users[uid].sendData(data);
                }
            } catch (Exception e) {
                throw e;
            }
        }
    }
}
