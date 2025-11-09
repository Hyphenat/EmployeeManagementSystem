using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: Reports Page
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        // GET: Employee Overview Report
        public async Task<IActionResult> EmployeeOverview()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var employees = await _context.Employees
                .Where(e => e.Role == "Employee")
                .ToListAsync();

            var reportData = new List<EmployeeOverviewDto>();

            foreach (var emp in employees)
            {
                var totalBonus = await _context.SalaryBonuses
                    .Where(b => b.EmployeeId == emp.EmployeeId)
                    .SumAsync(b => b.Amount);

                var totalSalaryPaid = await _context.Payslips
                    .Where(p => p.EmployeeId == emp.EmployeeId)
                    .SumAsync(p => p.NetSalary);

                reportData.Add(new EmployeeOverviewDto
                {
                    Employee = emp,
                    TotalBonus = totalBonus,
                    TotalSalaryPaid = totalSalaryPaid
                });
            }

            return View(reportData);
        }

        // GET: Monthly Attendance Report
        public async Task<IActionResult> MonthlyAttendance(int? month, int? year)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.SelectedMonth = month ?? DateTime.Now.Month;
            ViewBag.SelectedYear = year ?? DateTime.Now.Year;

            var employees = await _context.Employees
                .Where(e => e.Role == "Employee")
                .ToListAsync();

            var reportData = new List<MonthlyAttendanceDto>();

            foreach (var emp in employees)
            {
                var selectedMonth = (int)ViewBag.SelectedMonth;
                var selectedYear = (int)ViewBag.SelectedYear;

                var attendances = await _context.Attendances
                    .Where(a => a.EmployeeId == emp.EmployeeId &&
                           a.Date.Month == selectedMonth &&
                           a.Date.Year == selectedYear)
                    .ToListAsync();

                reportData.Add(new MonthlyAttendanceDto
                {
                    Employee = emp,
                    PresentDays = attendances.Count(a => a.Status == "Present"),
                    AbsentDays = attendances.Count(a => a.Status == "Absent"),
                    HalfDays = attendances.Count(a => a.Status == "Half-Day"),
                    LeaveDays = attendances.Count(a => a.Status == "Leave"),
                    TotalDays = DateTime.DaysInMonth(selectedYear, selectedMonth)
                });
            }

            return View(reportData);
        }

        // GET: Salary Report
        public async Task<IActionResult> SalaryReport(int? month, int? year)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;

            var payslips = await _context.Payslips
                .Include(p => p.Employee)
                .Where(p => p.Month == selectedMonth && p.Year == selectedYear)
                .ToListAsync();

            ViewBag.TotalBasicSalary = payslips.Sum(p => p.BasicSalary);
            ViewBag.TotalBonus = payslips.Sum(p => p.Bonus);
            ViewBag.TotalDeductions = payslips.Sum(p => p.Deductions);
            ViewBag.TotalNetSalary = payslips.Sum(p => p.NetSalary);

            return View(payslips);
        }
    }

    // DTOs for reports
    public class EmployeeOverviewDto
    {
        public Employee Employee { get; set; }
        public decimal TotalBonus { get; set; }
        public decimal TotalSalaryPaid { get; set; }
    }

    public class MonthlyAttendanceDto
    {
        public Employee Employee { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int HalfDays { get; set; }
        public int LeaveDays { get; set; }
        public int TotalDays { get; set; }
    }
}