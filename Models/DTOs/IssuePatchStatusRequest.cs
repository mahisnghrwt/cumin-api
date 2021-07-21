using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.DTOs {
    public class IssuePatchStatusRequest {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
