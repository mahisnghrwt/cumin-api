using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class ProjectInvitation {
        public int Id { get; set; }
        // inviter
        public int InviterId { get; set; }
        public User Inviter { get; set; }

        // invitee
        public int InviteeId { get; set; }
        public User Invitee { get; set; }

        // project
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // time stamp
        public DateTime InvitedAt { get; set; } = DateTime.UtcNow;
    }
}
