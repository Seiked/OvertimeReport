using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Overtime
{
    public class UserT
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string SolId { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string PositionTittle { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Office { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}
