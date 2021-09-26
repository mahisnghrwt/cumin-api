using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class Path {
        [Key]
        public int Id { get; set; }
        // fk
        public int FromEpicId { get; set; }
        [JsonIgnore]
        public Epic FromEpic { get; set; }
        // fk
        public int ToEpicId { get; set; }
        [JsonIgnore]
        public Epic ToEpic { get; set; }
        // fk
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
    }
}
