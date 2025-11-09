using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;
using System;

namespace EmployeeManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<SalaryBonus> SalaryBonuses { get; set; } = null!;
        public DbSet<Payslip> Payslips { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default admin user - FIXED: Using static date instead of DateTime.Now
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@company.com",
                    Password = "Admin@123",
                    PhoneNumber = "1234567890",
                    Position = "System Administrator",
                    Department = "IT",
                    BasicSalary = 100000,
                    JoiningDate = new DateTime(2024, 1, 1),
                    Role = "Admin",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            // Configure decimal precision
            modelBuilder.Entity<Employee>()
                .Property(e => e.BasicSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalaryBonus>()
                .Property(s => s.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payslip>()
                .Property(p => p.BasicSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payslip>()
                .Property(p => p.Bonus)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payslip>()
                .Property(p => p.Deductions)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payslip>()
                .Property(p => p.NetSalary)
                .HasPrecision(18, 2);

            // Configure relationships
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalaryBonus>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.SalaryBonuses)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payslip>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Payslips)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Employee)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}