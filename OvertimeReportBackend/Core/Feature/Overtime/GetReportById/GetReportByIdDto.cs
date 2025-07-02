using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Feature.Overtime.GetReportById
{
    public class GetReportByIdDto
    {
        public string SolId { get; set; } = string.Empty;
        public string SolName { get; set; } = string.Empty;
        public string CreationDate { get; set; } = string.Empty;
        public string Hour { get; set; } = string.Empty;
        public string Observations { get; set; } = string.Empty;
        public string SolHq { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string NoveltyDate { get; set; } = string.Empty;
        public string InitialHour { get; set; } = string.Empty;
        public string FinalHour { get; set; } = string.Empty;
        public string ReportHq { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string? OtherDetail { get; set; } = string.Empty;
        public bool? ByTicket { get; set; } = false;
        public bool? ByEmail { get; set; } = false;
        public string? TicketOrEmailInformation { get; set; } = string.Empty;
        public string ApprovalComment { get; set; } = string.Empty;
        public string ApprovalSolver { get; set; } = string.Empty;
    }
}