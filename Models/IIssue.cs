using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models {
    public interface IIssue {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Type { get; set; }
        int ProjectId { get; set; }
        int ReporterId { get; set; }
        int ResolverId { get; set; }
    }
}
