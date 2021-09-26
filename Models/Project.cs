using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Project {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // required
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        // fk, optional
        public int? ActiveSprintId { get; set; }
        [JsonIgnore]
        public Sprint ActiveSprint { get; set; }
        // navigation properties
        [JsonIgnore]
        public ICollection<UserProject> UserProjects { get; set; }
        [JsonIgnore]
        public ICollection<ProjectInvitation> ProjectInvitations { get; set; }
        [JsonIgnore]
        public ICollection<Issue> Issues { get; set; }
        [JsonIgnore]
        public ICollection<Sprint> Sprints { get; set; }
        [JsonIgnore]
        public ICollection<User> ActiveForUser { get; set; }
        [JsonIgnore]
        public ICollection<Epic> Epics { get; set; }
        [JsonIgnore]
        public ICollection<Path> Paths { get; set; }


        public Project DeepCopy() {
            return new Project { Id = Id, Name = Name, StartDate = StartDate, ActiveSprintId = ActiveSprintId };
        }
    }
}
