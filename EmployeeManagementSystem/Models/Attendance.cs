using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // "Present", "Absent", "Half-Day", "Leave"

        [StringLength(500)]
        public string? Remarks { get; set; }

        // Navigation property - FIXED
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}
