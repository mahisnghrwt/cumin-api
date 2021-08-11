using System.ComponentModel.DataAnnotations;

namespace cumin_api.Models.DTOs {
    public class IssueCreationDto {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int ReporterId { get; set; }
        public int? SprintId { get; set; }
    }
}
