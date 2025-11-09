using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check if user is admin
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        // GET: Admin Dashboard
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Dashboard statistics
            ViewBag.TotalEmployees = await _context.Employees.CountAsync(e => e.Role == "Employee");
            ViewBag.TotalSalary = await _context.Employees.Where(e => e.Role == "Employee").SumAsync(e => (decimal?)e.BasicSalary) ?? 0;
            ViewBag.TotalBonusPaid = await _context.SalaryBonuses.SumAsync(s => (decimal?)s.Amount) ?? 0;
            ViewBag.PendingFeedbacks = await _context.Feedbacks.CountAsync(f => f.Status == "Pending");
            ViewBag.TodayAttendance = await _context.Attendances.CountAsync(a => a.Date.Date == DateTime.Today && a.Status == "Present");

            return View();
        }

        // GET: Employee List
        public async Task<IActionResult> EmployeeList()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var employees = await _context.Employees
                .Where(e => e.Role == "Employee")
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();

            return View(employees);
        }

        // GET: Add Employee
        public IActionResult AddEmployee()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        // POST: Add Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Remove validation for navigation properties and auto-set fields
            ModelState.Remove("Attendances");
            ModelState.Remove("SalaryBonuses");
            ModelState.Remove("Payslips");
            ModelState.Remove("Feedbacks");
            ModelState.Remove("CreatedDate");

            // Ensure Role and IsActive are set
            employee.Role = "Employee";
            employee.IsActive = true;
            employee.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if email exists
                    var existingEmployee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Email == employee.Email);

                    if (existingEmployee != null)
                    {
                        ViewBag.Error = "Email already exists!";
                        return View(employee);
                    }

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Employee added successfully!";
                    return RedirectToAction("EmployeeList");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error adding employee: " + ex.Message;
                    return View(employee);  
                }
            }

            // Show validation errors if any
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            ViewBag.Error = "Validation failed: " + string.Join(", ", errors);
            return View(employee);
        }

        // GET: Edit Employee
        public async Task<IActionResult> EditEmployee(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: Edit Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Remove validation for navigation properties
            ModelState.Remove("Attendances");
            ModelState.Remove("SalaryBonuses");
            ModelState.Remove("Payslips");
            ModelState.Remove("Feedbacks");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Employee updated successfully!";
                    return RedirectToAction("EmployeeList");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error updating employee: " + ex.Message;
                }
            }

            return View(employee);
        }

        // POST: Delete Employee
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return Json(new { success = false, message = "Employee not found" });

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting employee: " + ex.Message });
            }
        }

        // GET: View Employee Details
        public async Task<IActionResult> EmployeeDetails(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var employee = await _context.Employees
                .Include(e => e.Attendances)
                .Include(e => e.SalaryBonuses)
                .Include(e => e.Payslips)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }
    }
}
