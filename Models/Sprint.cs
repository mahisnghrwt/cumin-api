using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cumin_api.Models {
    public class Sprint {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Issue> Issues { get; set; }


        [JsonIgnore]
        public Project Project { get; set; }
        [JsonIgnore]
        public Project ActiveForProject { get; set; }
        //[JsonIgnore]
    }
}
