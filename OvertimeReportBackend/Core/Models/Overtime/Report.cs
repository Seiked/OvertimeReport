using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Identity;

namespace Core.Models.Overtime
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public DateTime NoveltyDate { get; set; }
        [MaxLength(100)]
        public required string Headquarter { get; set; } = string.Empty;
        public required string InitialHour { get; set; }
        public required string FinalHour { get; set; }
        [MaxLength(100)]
        public string? Observations { get; set; } = string.Empty;
        public string? OtherDetail { get; set; } = string.Empty;
        public bool? ByTicket { get; set; } = false;
        public bool? ByEmail { get; set; } = false;
        public string? TicketOrEmailInformation { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ApprovalDate { get; set; }
        public string ApprovalComment { get; set; } = string.Empty;
        public string ApprovalSolver { get; set; } = string.Empty;
        public required int DetailId { get; set; }
        public string ActiveTrackFirstActivity { get; set; } = string.Empty;
        public string ActiveTrackLastActivity { get; set; } = string.Empty;
        public Detail Detail { get; set; }
        public required string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public required int StatusId { get; set; }
        public Status Status { get; set; }

    }
}