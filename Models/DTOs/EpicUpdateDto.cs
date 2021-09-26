using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class EpicUpdateDto {
        public string Title { get; set; } = "EMPTY";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Color { get; set; } = "EMPTY";
    }
}
