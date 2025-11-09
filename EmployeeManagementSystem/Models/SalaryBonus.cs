using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class SalaryBonus
    {
        [Key]
        public int BonusId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string BonusType { get; set; } = string.Empty; // "Performance", "Festival", "Annual", etc.

        [Required]
        [Range(0, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime BonusDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property - FIXED
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}