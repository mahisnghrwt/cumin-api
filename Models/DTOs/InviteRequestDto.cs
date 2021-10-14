using System.ComponentModel.DataAnnotations;

namespace cumin_api.Models.DTOs {
    public class InviteRequestDto {
        [Required]
        public string Username { get; set; }
    }
}
