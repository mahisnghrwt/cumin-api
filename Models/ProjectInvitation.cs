using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class ProjectInvitation {
        [Key]
        public int Id { get; set; }
        // time stamp
        public DateTime InvitedAt { get; set; } = DateTime.UtcNow;
        // fk
        public int InviterId { get; set; }
        public User Inviter { get; set; }
        // fk
        public int InviteeId { get; set; }
        public User Invitee { get; set; }
        // fk
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
