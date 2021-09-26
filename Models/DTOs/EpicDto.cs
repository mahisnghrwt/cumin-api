using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class EpicDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public int RoadmapId { get; set; }
        public int Row { get; set; }
        public string Color { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}
