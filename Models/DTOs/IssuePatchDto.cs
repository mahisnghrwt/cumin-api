using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class IssuePatchDto {
        public string Title { get; set; } = "EMPTY";
        public string Description { get; set; } = "EMPTY";
        public string Type { get; set; } = "EMPTY";
        public string Status { get; set; } = "EMPTY";
        public int? AssignedToId { get; set; }
        public int? SprintId { get; set; }
        public int? EpicId { get; set; }
    }
}
