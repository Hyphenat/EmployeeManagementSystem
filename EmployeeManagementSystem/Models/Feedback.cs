using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        [Required]
        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // "Pending", "Reviewed", "Resolved"

        [StringLength(1000)]
        public string? AdminResponse { get; set; }

        // Navigation property - FIXED
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}