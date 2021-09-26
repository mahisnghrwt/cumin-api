using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Issue {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } // required
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } // required
        public string Status { get; set; } = "Todo"; // required
        //fk
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        //fk
        public int ReporterId { get; set; }
        [JsonIgnore]
        public User Reporter { get; set; }
        //fk
        public int? AssignedToId { get; set; }
        [JsonIgnore]
        public User AssignedTo { get; set; }
        //fk
        public int? SprintId { get; set; }
        [JsonIgnore]
        public Sprint Sprint { get; set; }
        //fk
        public int? EpicId { get; set; }
        [JsonIgnore]
        public Epic Epic { get; set; }

        public Issue() { }
        public Issue(int id, string title, string desc, DateTime createdAt, 
            string type, string status, int projectId, int reporterId, int? assignedToId, int? sprintId) {
            Id = id;
            Title = title;
            Description = desc;
            CreatedAt = createdAt;
            Type = type;
            Status = status;
            ProjectId = projectId;
            ReporterId = reporterId;
            AssignedToId = assignedToId;
            SprintId = sprintId;
        }

        public Issue(Issue issue) : this(issue.Id, issue.Title, issue.Description, issue.CreatedAt,
            issue.Type, issue.Status, issue.ProjectId, issue.ReporterId, issue.AssignedToId, issue.SprintId) { }

        public void CopyForUpdate(Issue target) {
            Title = target.Title;
            Description = target.Description;
            Type = target.Type;
            Status = target.Status;
            SprintId = target.SprintId;
        }

    }
}
