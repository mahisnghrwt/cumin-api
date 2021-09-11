using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class RoadmapEpic {
        [Key]
        public int Id { get; set; }
        public int RoadmapId { get; set; }
        public Roadmap Roadmap { get; set; }
        public int EpicId { get; set; }
        public Epic Epic { get; set; }
    }
}
