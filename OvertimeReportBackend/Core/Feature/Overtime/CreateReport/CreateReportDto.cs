using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Feature.Overtime.CreateReport
{
    public class CreateReportDto
    {
        public DateTime NoveltyDate { get; set; }
        public required string Headquarter { get; set; } = string.Empty;
        public required string InitialHour { get; set; }
        public required string FinalHour { get; set; }
        public string? Observations { get; set; } = string.Empty;
        public string? OtherDetail { get; set; } = string.Empty;
        public bool? ByTicket { get; set; } = false;
        public bool? ByEmail { get; set; } = false;
        public string? TicketOrEmailInformation { get; set; } = string.Empty;
        public required int DetailId { get; set; }
    }
}