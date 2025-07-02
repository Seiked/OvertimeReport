using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Feature.Overtime.GetAllReportUser
{
    public class GetAllReportUserDto
    {
        public int Id { get; set; }
        public string ReportDate { get; set; } = string.Empty;
        public string Headquarter { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string Hour { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ApprovalDate { get; set; } = string.Empty;
    }
}