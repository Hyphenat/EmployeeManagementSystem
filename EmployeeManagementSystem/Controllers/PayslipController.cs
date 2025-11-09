using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class PayslipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayslipController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: Payslip List (Admin)
        public async Task<IActionResult> Index(int? month, int? year)
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
                .OrderBy(p => p.Employee.FirstName)
                .ToListAsync();

            return View(payslips);
        }

        // GET: Generate Payslip
        public async Task<IActionResult> GeneratePayslip()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Employees = await _context.Employees
                .Where(e => e.Role == "Employee" && e.IsActive)
                .ToListAsync();

            return View();
        }

        // POST: Generate Payslip
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneratePayslip(int employeeId, int month, int year)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    TempData["Error"] = "Employee not found!";
                    return RedirectToAction("GeneratePayslip");
                }

                // Check if payslip already exists
                var existingPayslip = await _context.Payslips
                    .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year);

                if (existingPayslip != null)
                {
                    TempData["Error"] = "Payslip already generated for this month!";
                    return RedirectToAction("GeneratePayslip");
                }

                // Calculate working days and present days
                var daysInMonth = DateTime.DaysInMonth(year, month);
                var attendances = await _context.Attendances
                    .Where(a => a.EmployeeId == employeeId && a.Date.Month == month && a.Date.Year == year)
                    .ToListAsync();

                var presentDays = attendances.Count(a => a.Status == "Present");
                var halfDays = attendances.Count(a => a.Status == "Half-Day");
                var effectiveDays = presentDays + (halfDays * 0.5m);

                // Calculate bonus for this month
                var monthlyBonus = await _context.SalaryBonuses
                    .Where(b => b.EmployeeId == employeeId && b.BonusDate.Month == month && b.BonusDate.Year == year)
                    .SumAsync(b => b.Amount);

                // Calculate salary
                var perDaySalary = employee.BasicSalary / daysInMonth;
                var earnedSalary = perDaySalary * effectiveDays;
                var deductions = employee.BasicSalary - earnedSalary;
                var netSalary = earnedSalary + monthlyBonus;

                // Create payslip
                var payslip = new Payslip
                {
                    EmployeeId = employeeId,
                    Month = month,
                    Year = year,
                    BasicSalary = employee.BasicSalary,
                    Bonus = monthlyBonus,
                    Deductions = deductions,
                    NetSalary = netSalary,
                    WorkingDays = daysInMonth,
                    PresentDays = (int)effectiveDays,
                    GeneratedDate = DateTime.Now,
                    Status = "Generated"
                };

                _context.Payslips.Add(payslip);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Payslip generated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error generating payslip: " + ex.Message;
                return RedirectToAction("GeneratePayslip");
            }
        }

        // GET: View Payslip
        public async Task<IActionResult> ViewPayslip(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var payslip = await _context.Payslips
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.PayslipId == id);

            if (payslip == null)
                return NotFound();

            // Check authorization
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "Admin" && userId != payslip.EmployeeId)
                return RedirectToAction("Login", "Account");

            return View(payslip);
        }

        // POST: Delete Payslip
        [HttpPost]
        public async Task<IActionResult> DeletePayslip(int id)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Unauthorized" });

            var payslip = await _context.Payslips.FindAsync(id);
            if (payslip == null)
                return Json(new { success = false, message = "Payslip not found" });

            _context.Payslips.Remove(payslip);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Payslip deleted successfully" });
        }
    }
}