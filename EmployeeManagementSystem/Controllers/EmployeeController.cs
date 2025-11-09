using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        private bool IsEmployee()
        {
            return HttpContext.Session.GetString("UserRole") == "Employee";
        }

        // GET: Employee Dashboard
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var employee = await _context.Employees.FindAsync(userId);
            ViewBag.Employee = employee;

            // Today's attendance
            var todayAttendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == userId && a.Date.Date == DateTime.Today);
            ViewBag.TodayAttendance = todayAttendance;

            // This month statistics
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var monthAttendances = await _context.Attendances
                .Where(a => a.EmployeeId == userId && a.Date.Month == currentMonth && a.Date.Year == currentYear)
                .ToListAsync();

            ViewBag.PresentDays = monthAttendances.Count(a => a.Status == "Present");
            ViewBag.AbsentDays = monthAttendances.Count(a => a.Status == "Absent");
            ViewBag.TotalDays = DateTime.DaysInMonth(currentYear, currentMonth);

            // Latest payslip
            var latestPayslip = await _context.Payslips
                .Where(p => p.EmployeeId == userId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .FirstOrDefaultAsync();
            ViewBag.LatestPayslip = latestPayslip;

            return View();
        }

        // GET: My Profile
        public async Task<IActionResult> MyProfile()
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var employee = await _context.Employees.FindAsync(userId);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: Update Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyProfile(Employee employee)
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            if (employee.EmployeeId != userId)
                return RedirectToAction("Login", "Account");

            try
            {
                var existingEmployee = await _context.Employees.FindAsync(userId);
                if (existingEmployee == null)
                    return NotFound();

                // Update only allowed fields
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Email = employee.Email;

                _context.Update(existingEmployee);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction("MyProfile");
            }
            catch
            {
                ViewBag.Error = "Error updating profile!";
                return View(employee);
            }
        }

        // GET: Change Password
        public IActionResult ChangePassword()
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        // POST: Change Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                ViewBag.Error = "All fields are required!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New password and confirm password do not match!";
                return View();
            }

            var employee = await _context.Employees.FindAsync(userId);
            if (employee == null)
                return NotFound();

            if (employee.Password != currentPassword)
            {
                ViewBag.Error = "Current password is incorrect!";
                return View();
            }

            employee.Password = newPassword;
            _context.Update(employee);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("Index");
        }

        // GET: My Salary Details
        public async Task<IActionResult> MySalary(int? month, int? year)
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedMonth = month ?? DateTime.Now.Month;
            ViewBag.SelectedYear = selectedYear;

            var payslips = await _context.Payslips
                .Where(p => p.EmployeeId == userId && p.Year == selectedYear)
                .OrderBy(p => p.Month)
                .ToListAsync();

            var employee = await _context.Employees.FindAsync(userId);
            ViewBag.Employee = employee;

            return View(payslips);
        }

        // GET: My Attendance
        public async Task<IActionResult> MyAttendance(int? month, int? year)
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;

            var attendances = await _context.Attendances
                .Where(a => a.EmployeeId == userId && a.Date.Month == selectedMonth && a.Date.Year == selectedYear)
                .OrderBy(a => a.Date)
                .ToListAsync();

            return View(attendances);
        }

        // GET: Submit Feedback
        public IActionResult SubmitFeedback()
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        // POST: Submit Feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(Feedback feedback)
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            if (ModelState.IsValid)
            {
                feedback.EmployeeId = userId.Value;
                feedback.SubmittedDate = DateTime.Now;
                feedback.Status = "Pending";

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Feedback submitted successfully!";
                return RedirectToAction("MyFeedbacks");
            }

            return View(feedback);
        }

        // GET: My Feedbacks
        public async Task<IActionResult> MyFeedbacks()
        {
            var userId = GetUserId();
            if (userId == null || !IsEmployee())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var feedbacks = await _context.Feedbacks
                .Where(f => f.EmployeeId == userId)
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();

            return View(feedbacks);
        }
    }
}
