using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.SocketMsgs {
    public class GeneralSockMsg: SockMsg {
        public int eventName { get; set; }
    }
}
