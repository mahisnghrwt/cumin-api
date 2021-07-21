using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Models.Requests {
    public class IssueRequest : IIssue {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int ProjectId { get; set; }
        public int ReporterId { get; set; }
        public int ResolverId { get; set; }
    }
}
