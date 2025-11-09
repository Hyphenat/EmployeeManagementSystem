using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Payslip
    {
        [Key]
        public int PayslipId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        public decimal Bonus { get; set; } = 0;

        public decimal Deductions { get; set; } = 0;

        [Required]
        public decimal NetSalary { get; set; }

        public int WorkingDays { get; set; }

        public int PresentDays { get; set; }

        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Status { get; set; } = "Generated"; // "Generated", "Paid"

        // Navigation property - FIXED
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}