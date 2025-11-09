using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(100)]
        public string Position { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100)]
        public string Department { get; set; }

        [Required]
        [Range(0, 10000000, ErrorMessage = "Salary must be between 0 and 10,000,000")]
        public decimal BasicSalary { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // "Admin" or "Employee"

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties - FIXED
        public virtual ICollection<Attendance>? Attendances { get; set; }
        public virtual ICollection<SalaryBonus>? SalaryBonuses { get; set; }
        public virtual ICollection<Payslip>? Payslips { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}