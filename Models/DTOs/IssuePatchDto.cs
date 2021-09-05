using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class IssuePatchDto {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Type { get; set; }
        public string Status { get; set; } = "Todo";
        //public int ProjectId { get; set; }
        //public Project Project { get; set; }
        //public User Reporter { get; set; }
        //public User Resolver { get; set; }

        // can be nulls
        public string Description { get; set; }
        public int? ResolverId { get; set; }
        public int? SprintId { get; set; }
        public int? EpicId { get; set; }


        //[JsonIgnore]
        //public Sprint Sprint { get; set; }

        //public IssuePatchDto() { }
        //public IssuePatchDto(int id, string title, string desc, DateTime createdAt, 
        //    string type, string status, int projectId, int reporterId, int? resolverId, int? sprintId) {
        //    Id = id;
        //    Title = title;
        //    Description = desc;
        //    CreatedAt = createdAt;
        //    Type = type;
        //    Status = status;
        //    ProjectId = projectId;
        //    ReporterId = reporterId;
        //    ResolverId = resolverId;
        //    SprintId = sprintId;
        //}

        //public IssuePatchDto(IssuePatchDto issue) : this(issue.Id, issue.Title, issue.Description, issue.CreatedAt,
        //    issue.Type, issue.Status, issue.ProjectId, issue.ReporterId, issue.ResolverId, issue.SprintId) { }

        //public void CopyForUpdate(IssuePatchDto target) {
        //    Title = target.Title;
        //    Description = target.Description;
        //    Type = target.Type;
        //    Status = target.Status;
        //    SprintId = target.SprintId;
        //}

    }
}
