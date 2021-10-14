using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Epic {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } // required
        public DateTime StartDate { get; set; } // required
        public DateTime EndDate { get; set; } // required
        public string Color { get; set; } // required
        public int Row { get; set; } // required
        // fk
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        // navigation properties
        [JsonIgnore]
        public ICollection<Path> PathsFrom { get; set; }
        [JsonIgnore]
        public ICollection<Path> PathsTo { get; set; }
        public ICollection<Issue> Issues { get; set; }


        public Epic() { }
        public Epic(Epic epic) {
            Id = epic.Id;
            Title = epic.Title;
            StartDate = epic.StartDate;
            EndDate = epic.EndDate;
            ProjectId = epic.ProjectId;
            Color = epic.Color;
        }
    }
}
