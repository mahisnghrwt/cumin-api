using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Roadmap {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public int? CreatorId { get; set; }
        public int Rows { get; set; } = 0;
        [JsonIgnore]
        public User Creator { get; set; }
        public ICollection<RoadmapEpic> RoadmapEpics { get; set; }
        public ICollection<RoadmapPath> RoadmapPaths { get; set; }
    }
}
