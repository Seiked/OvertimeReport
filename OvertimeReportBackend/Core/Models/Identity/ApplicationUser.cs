using Core.Models.Overtime;
using Microsoft.AspNetCore.Identity;

namespace Core.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string SolId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PositionTittle { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
    }
}