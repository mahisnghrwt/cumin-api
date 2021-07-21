using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class ActiveSprintProject {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project {get; set;}
        public int SprintId { get; set; }
        public Sprint Sprint { get; set; }
    }
}
