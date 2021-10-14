using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cumin_api.Models {
    public class Sprint {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        // fk
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        // navigation properties
        [JsonIgnore]
        public Project ActiveForProject { get; set; }
        public ICollection<Issue> Issues { get; set; }
        
    }
}
