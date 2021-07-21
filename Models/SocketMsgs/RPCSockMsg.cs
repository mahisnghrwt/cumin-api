using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.SocketMsgs {
    public class RPCSockMsg : SockMsg {
        public RPCSockMsg(): base() {
            id = -1;
            type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.RPC;
        }
    }
}
