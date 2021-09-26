using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class UserProject {
        [Key]
        public int Id { get; set; }
        public string UserRole { get; set; }
        // foreign key
        public int UserId { get; set; }
        public User User { get; set; }
        // foreign key
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
