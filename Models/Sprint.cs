using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Sprint {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;        
        public ICollection<Issue> Issues { get; set; }
        public int? ActiveSprintId { get; set; }

        [JsonIgnore]
        public ActiveSprintProject ActiveSprint { get; set; }

        public Sprint() { }

        public Sprint(Sprint sprint) : this(sprint.Id, sprint.Title, sprint.ProjectId, sprint.CreatedAt, sprint.ActiveSprintId) { }
        public Sprint(int id, string title, int projectId, 
            DateTime createdAt, int? activeSprintId) {
            Id = id;
            Title = title;
            ProjectId = projectId;
            CreatedAt = createdAt;
            ActiveSprintId = activeSprintId;
        }
    }
}
