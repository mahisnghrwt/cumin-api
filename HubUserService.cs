using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api {
    public class HubUserService {
        public Dictionary<int, string> Users { get; }

        public HubUserService() {
            this.Users = new Dictionary<int, string>();
        }
    }
}
