using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class ProjectDto {
        public int Id { get; set; }
        public string Name { get; set; } // required
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public int? ProjectManagerId { get; set; }    
    }
}
