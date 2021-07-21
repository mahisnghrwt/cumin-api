using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class ProjectInvitationRequest {
        public string InviteeUsername { get; set; }
        public int ProjectId { get; set; }
    }
}
