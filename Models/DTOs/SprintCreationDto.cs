using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class SprintCreationDto {
        [Required]
        public string Title { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
