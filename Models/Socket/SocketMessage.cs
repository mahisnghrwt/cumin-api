using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.Socket {
    public class SocketMessage {
        public int eventName { get; set; }
        public Object payload { get; set; }
    }
}
