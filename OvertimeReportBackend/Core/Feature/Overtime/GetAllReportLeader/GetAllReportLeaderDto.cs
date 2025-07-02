using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Feature.Overtime.GetAllReportLeader
{
    public class GetAllReportLeaderDto
    {
        public int Id { get; set; }
        public string NotificationDate { get; set; } = string.Empty;
        public string SolId { get; set; } = string.Empty;
        public string SolName { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string ReportDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}