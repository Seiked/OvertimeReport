using System.ComponentModel.DataAnnotations;

namespace Core.Models.Overtime
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
    }
}