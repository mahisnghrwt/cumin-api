using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.SocketMsgs {
    public class SockMsg {
        public int id { get; set; }
        public int type { get; set; }
        public Object payload { get; set; }

    }
}
