using cumin_api.Services.v2;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api {
    public class NotificationHub: Hub {
        private readonly UserService userService;
        private readonly HubUserService hubUserService;

        public NotificationHub(UserService userService, HubUserService hubUserService) {
            this.userService = userService;
            this.hubUserService = hubUserService;
        }

        public override async Task OnConnectedAsync() {
            int userId = Convert.ToInt32(Context.GetHttpContext().Items["userId"]);
            hubUserService.Users[userId] = Context.ConnectionId;

            int? groupId = (await userService.FindAsync(x => x.Id == userId)).ActiveProjectId;
            // if user has an active project, add it to that group
            if (groupId != null) {
                await Groups.AddToGroupAsync(Context.ConnectionId, Convert.ToString(groupId));
            }
            await base.OnConnectedAsync();
        }
    }
}
