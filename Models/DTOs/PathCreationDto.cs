using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class PathCreationDto {
        public int FromEpicId { get; set; }
        public int ToEpicId { get; set; }
    }
}
