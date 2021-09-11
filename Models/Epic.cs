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
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Row { get; set; }
        public int ProjectId { get; set; }

        [JsonIgnore]
        public Project Project { get; set; }
        [JsonIgnore]
        public ICollection<Path> PathsFrom { get; set; }
        [JsonIgnore]
        public ICollection<Path> PathsTo { get; set; }
        [JsonIgnore]
        public ICollection<Issue> Issues { get; set; }
        [JsonIgnore]
        public ICollection<RoadmapEpic> RoadmapEpics { get; set; }


        public Epic() { }
        public Epic(Epic epic) {
            Id = epic.Id;
            Title = epic.Title;
            StartDate = epic.StartDate;
            EndDate = epic.EndDate;
            Row = epic.Row;
            ProjectId = epic.ProjectId;
        }
    }
}
