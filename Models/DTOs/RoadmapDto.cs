using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class RoadmapDto {
        public int ProjectId { get; set; }
        public IEnumerable<Epic> Epics { get; set; }
        public IEnumerable<Path> Paths { get; set; }
    }
}
