using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.Socket {
    public class SocketMessageHeader {
        public int targetId { get; set; }
        public int broadcastType { get; set; }
    }
}
