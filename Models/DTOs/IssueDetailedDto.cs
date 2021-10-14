using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class IssueDetailedDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public ProjectBriefDto Project { get; set; }
        public UserBriefDto Reporter { get; set; }
        public UserBriefDto AssignedTo { get; set; }
        public SprintBriefDto Sprint { get; set; }
        public EpicBriefDto Epic { get; set; }

    }
}
