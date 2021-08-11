using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public class UserAuthenticationDto {
        [Required, StringLength(10)]
        public string Username { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }
    }
}
