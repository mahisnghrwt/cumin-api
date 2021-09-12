using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Path {
        public int Id { get; set; }
        public int FromEpicId { get; set; }
        public int ToEpicId { get; set; }
        public int ProjectId { get; set; }

        [JsonIgnore]
        public Epic FromEpic { get; set; }
        [JsonIgnore]
        public Epic ToEpic { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        [JsonIgnore]
        public ICollection<RoadmapPath> RoadmapPaths { get; set; }
    }
}
