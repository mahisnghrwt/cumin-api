using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class User {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int? ActiveProjectId { get; set; }
        public Project ActiveProject { get; set; }

        [JsonIgnore]
        public ICollection<UserProject> UserProjects { get; set; }
        [JsonIgnore]
        public ICollection<ProjectInvitation> ProjectInvitationSent { get; set; }
        [JsonIgnore]
        public ICollection<ProjectInvitation> ProjectInvitedTo { get; set; }
        [JsonIgnore]
        public ICollection<Issue> IssueReporter { get; set; }
        [JsonIgnore]
        public ICollection<Issue> IssueAssigned { get; set; }
    }
}
